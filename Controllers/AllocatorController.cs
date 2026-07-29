// File: Controllers/AllocatorController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Data;
using RefineryBooking.Models;
using ClosedXML.Excel;
using System.Security.Claims;

namespace RefineryBooking.Controllers
{
    [Authorize(Roles = "Allocator,Admin")]
    public class AllocatorController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AllocatorController(ApplicationDbContext context) => _context = context;

        // ── Helper: get room IDs assigned to the current allocator ────────────────
        // Admins bypass hall scoping and see all bookings.
        private async Task<List<int>?> GetAssignedRoomIdsAsync()
        {
            if (User.IsInRole("Admin")) return null; // null = no filter (all halls)

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.AllocatorHallAssignments
                .Where(a => a.AllocatorUserId == userId)
                .Select(a => a.ConferenceRoomId)
                .ToListAsync();
        }

        // ── TABBED INDEX ─────────────────────────────────────────────────────
        // tab: "review" (default for Allocator) | "pending" | "approved" | "rejected" | "all"
        public async Task<IActionResult> Index(string tab = "review")
        {
            ViewBag.ActiveTab = tab;

            var assignedRoomIds = await GetAssignedRoomIdsAsync();

            // Warn allocator if they have no halls assigned yet
            if (assignedRoomIds != null && assignedRoomIds.Count == 0)
            {
                ViewBag.NoHallsAssigned = true;
                ViewBag.CountReview   = 0;
                ViewBag.CountPending  = 0;
                ViewBag.CountApproved = 0;
                ViewBag.CountRejected = 0;
                return View(new List<Booking>());
            }

            var query = _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.User)
                .Include(b => b.ITRequirement)
                .AsQueryable();

            // Scope to assigned halls (null = Admin, sees all)
            if (assignedRoomIds != null)
                query = query.Where(b => assignedRoomIds.Contains(b.ConferenceRoomId));

            IQueryable<Booking> filtered = tab switch
            {
                "pending"  => query.Where(b => b.Status == BookingStatus.Pending),
                "approved" => query.Where(b => b.Status == BookingStatus.Approved),
                "rejected" => query.Where(b => b.Status == BookingStatus.Rejected),
                "all"      => query,
                _          => query.Where(b => b.Status == BookingStatus.PendingAllocatorReview)
            };

            var bookings = await filtered.OrderBy(b => b.StartTime).ToListAsync();

            // Tab counts scoped to assigned halls
            var scopedBase = assignedRoomIds != null
                ? _context.Bookings.Where(b => assignedRoomIds.Contains(b.ConferenceRoomId))
                : _context.Bookings;

            ViewBag.CountReview   = await scopedBase.CountAsync(b => b.Status == BookingStatus.PendingAllocatorReview);
            ViewBag.CountPending  = await scopedBase.CountAsync(b => b.Status == BookingStatus.Pending);
            ViewBag.CountApproved = await scopedBase.CountAsync(b => b.Status == BookingStatus.Approved);
            ViewBag.CountRejected = await scopedBase.CountAsync(b => b.Status == BookingStatus.Rejected);

            return View(bookings);
        }

        // ── DETAILS ──────────────────────────────────────────────────────────
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.User)
                .Include(b => b.ITRequirement)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            // Verify allocator is assigned to this booking's hall
            var assignedRoomIds = await GetAssignedRoomIdsAsync();
            if (assignedRoomIds != null && !assignedRoomIds.Contains(booking.ConferenceRoomId))
                return Forbid();

            // Check for conflicts
            ViewBag.HasConflict = await _context.Bookings.AnyAsync(b =>
                b.Id != id &&
                b.ConferenceRoomId == booking.ConferenceRoomId &&
                b.Status == BookingStatus.Approved &&
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime);

            return View(booking);
        }

        // ── APPROVE ──────────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            // Verify allocator is assigned to this hall
            var assignedRoomIds = await GetAssignedRoomIdsAsync();
            if (assignedRoomIds != null && !assignedRoomIds.Contains(booking.ConferenceRoomId))
                return Forbid();

            bool conflict = await _context.Bookings.AnyAsync(b =>
                b.Id != id &&
                b.ConferenceRoomId == booking.ConferenceRoomId &&
                b.Status == BookingStatus.Approved &&
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime);

            if (conflict)
            {
                TempData["ErrorMessage"] = "Cannot approve: A confirmed booking already occupies this time slot.";
                return RedirectToAction(nameof(Details), new { id });
            }

            booking.Status = BookingStatus.Approved;
            _context.AuditLogs.Add(new AuditLog
            {
                UserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name)?.Id,
                UserName = User.Identity?.Name,
                Action = "APPROVE_BOOKING",
                EntityName = "Booking",
                EntityId = id.ToString(),
                Details = $"Approved BKG-{id}: '{booking.MeetingTitle}'"
            });

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"BKG-{id} has been approved.";
            return RedirectToAction(nameof(Index));
        }

        // ── REJECT ───────────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            // Verify allocator is assigned to this hall
            var assignedRoomIds = await GetAssignedRoomIdsAsync();
            if (assignedRoomIds != null && !assignedRoomIds.Contains(booking.ConferenceRoomId))
                return Forbid();

            booking.Status = BookingStatus.Rejected;
            booking.RejectionReason = string.IsNullOrWhiteSpace(reason) ? "Not specified by Allocator." : reason;

            _context.AuditLogs.Add(new AuditLog
            {
                UserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name)?.Id,
                UserName = User.Identity?.Name,
                Action = "REJECT_BOOKING",
                EntityName = "Booking",
                EntityId = id.ToString(),
                Details = $"Rejected BKG-{id}. Reason: {booking.RejectionReason}"
            });

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"BKG-{id} has been rejected.";
            return RedirectToAction(nameof(Index));
        }

        // ── CANCEL (Admin only) ──────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int id, string reason)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.Status = BookingStatus.Cancelled;
            booking.RejectionReason = reason;

            _context.AuditLogs.Add(new AuditLog
            {
                UserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name)?.Id,
                UserName = User.Identity?.Name,
                Action = "CANCEL_BOOKING",
                EntityName = "Booking",
                EntityId = id.ToString(),
                Details = $"Cancelled BKG-{id}. Reason: {reason}"
            });

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"BKG-{id} has been cancelled.";
            return RedirectToAction(nameof(Index));
        }

        // ── EXPORT TO EXCEL ──────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Export(string tab = "all")
        {
            var assignedRoomIds = await GetAssignedRoomIdsAsync();

            var query = _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.User)
                .AsQueryable();

            // Scope to assigned halls
            if (assignedRoomIds != null)
                query = query.Where(b => assignedRoomIds.Contains(b.ConferenceRoomId));

            IQueryable<Booking> filtered = tab switch
            {
                "pending"  => query.Where(b => b.Status == BookingStatus.Pending),
                "approved" => query.Where(b => b.Status == BookingStatus.Approved),
                "rejected" => query.Where(b => b.Status == BookingStatus.Rejected),
                "review"   => query.Where(b => b.Status == BookingStatus.PendingAllocatorReview),
                _          => query
            };

            var bookings = await filtered.OrderBy(b => b.StartTime).ToListAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Bookings");

            // Header row
            string[] headers = {
                "Booking ID", "Meeting Title", "Organizer", "Department", "Cost Centre",
                "Hall", "Building", "Date", "Start Time", "End Time", "Participants",
                "Status", "Help: ITFM?", "ITFM Help Details",
                "Help: Allocator?", "Allocator Help Details",
                "Remarks", "Submitted On"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#0b2553");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Data rows
            for (int r = 0; r < bookings.Count; r++)
            {
                var b = bookings[r];
                var row = r + 2;
                ws.Cell(row, 1).Value  = $"BKG-{b.Id}";
                ws.Cell(row, 2).Value  = b.MeetingTitle;
                ws.Cell(row, 3).Value  = b.OrganizerName;
                ws.Cell(row, 4).Value  = b.Department;
                ws.Cell(row, 5).Value  = b.CostCentre;
                ws.Cell(row, 6).Value  = b.ConferenceRoom?.Name ?? "";
                ws.Cell(row, 7).Value  = b.ConferenceRoom?.BuildingLocation ?? "";
                ws.Cell(row, 8).Value  = b.StartTime.ToString("dd MMM yyyy");
                ws.Cell(row, 9).Value  = b.StartTime.ToString("HH:mm");
                ws.Cell(row, 10).Value = b.EndTime.ToString("HH:mm");
                ws.Cell(row, 11).Value = b.ExpectedAttendees;
                ws.Cell(row, 12).Value = b.Status.ToString();
                ws.Cell(row, 13).Value = b.RequiresITFMHelp ? "Yes" : "No";
                ws.Cell(row, 14).Value = b.ITFMHelpDetails ?? "";
                ws.Cell(row, 15).Value = b.RequiresAllocatorHelp ? "Yes" : "No";
                ws.Cell(row, 16).Value = b.AllocatorHelpDetails ?? "";
                ws.Cell(row, 17).Value = b.Remarks ?? "";
                ws.Cell(row, 18).Value = b.CreatedAt.ToLocalTime().ToString("dd MMM yyyy HH:mm");

                // Colour-code status
                var statusCell = ws.Cell(row, 12);
                statusCell.Style.Fill.BackgroundColor = b.Status switch
                {
                    BookingStatus.Approved               => XLColor.FromHtml("#d4edda"),
                    BookingStatus.Rejected               => XLColor.FromHtml("#f8d7da"),
                    BookingStatus.Cancelled              => XLColor.FromHtml("#e2e3e5"),
                    BookingStatus.PendingAllocatorReview => XLColor.FromHtml("#cce5ff"),
                    _                                    => XLColor.FromHtml("#fff3cd")
                };

                // Alternate row shading
                if (r % 2 == 1)
                {
                    for (int c = 1; c <= headers.Length; c++)
                    {
                        if (c != 12)
                            ws.Cell(row, c).Style.Fill.BackgroundColor = XLColor.FromHtml("#f7f9fc");
                    }
                }
            }

            ws.Columns().AdjustToContents();
            ws.Column(1).Width = 12;
            ws.SheetView.FreezeRows(1);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var filename = $"NRL_Bookings_{tab}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                filename);
        }
    }
}