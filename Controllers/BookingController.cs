// File: Controllers/BookingController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Data;
using RefineryBooking.Models;
using System.Security.Claims;

namespace RefineryBooking.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookings = await _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.ITRequirement)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();

            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? roomId, string? date)
        {
            ViewBag.Rooms = new SelectList(await _context.ConferenceRooms.Where(r => r.IsActive).ToListAsync(), "Id", "Name", roomId);
            var activeCostCentres = await _context.CostCentres.Where(c => c.IsActive).OrderBy(c => c.Name).Select(c => new { Code = c.Code, Display = c.Code + " - " + c.Name }).ToListAsync();
            ViewBag.CostCentres = new SelectList(activeCostCentres, "Code", "Display");

            var model = new Booking();
            if (roomId.HasValue && !string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var parsedDate))
            {
                model.ConferenceRoomId = roomId.Value;
                model.StartTime = parsedDate.AddHours(9);
                model.EndTime = parsedDate.AddHours(10);
            }
            else
            {
                model.StartTime = DateTime.Today.AddDays(1).AddHours(9);
                model.EndTime = DateTime.Today.AddDays(1).AddHours(10);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                model.Department = user.Department;
                model.OrganizerName = user.FullName;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking, ITFacilityRequirement itReq, IFormFile? attachment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            // 1. Assign backend data immediately
            booking.UserId = userId;
            booking.Status = User.IsInRole("Admin") ? BookingStatus.Approved : BookingStatus.Pending;
            booking.CreatedAt = DateTime.UtcNow;

            // 2. Clear validation checks for properties set programmatically
            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("ConferenceRoom");
            ModelState.Remove("ITRequirement");
            ModelState.Remove("itReq.Booking");
            ModelState.Remove("itReq.BookingId");

            // 3. Working Hours Validation (09:30 – 17:30)
            var startWorkTime = new TimeSpan(9, 30, 0);
            var endWorkTime = new TimeSpan(17, 30, 0);

            if (booking.StartTime.TimeOfDay < startWorkTime || booking.EndTime.TimeOfDay > endWorkTime)
            {
                ModelState.AddModelError("", "Bookings must be within working hours: 09:30 \u2013 17:30.");
            }

            // 4. Time range validation
            if (booking.StartTime >= booking.EndTime)
            {
                ModelState.AddModelError("EndTime", "End time must be after start time.");
            }

            if (booking.StartTime < DateTime.Now)
            {
                ModelState.AddModelError("StartTime", "Cannot book meetings in the past.");
            }

            // 5. Real-time Database Conflict Check
            bool isDoubleBooked = await _context.Bookings.AnyAsync(b =>
                b.ConferenceRoomId == booking.ConferenceRoomId &&
                b.Status != BookingStatus.Rejected &&
                b.Status != BookingStatus.Cancelled &&
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime
            );

            if (isDoubleBooked)
            {
                ModelState.AddModelError("", "CRITICAL CONFLICT: This room is already booked for the selected timeframe. Please select another slot.");
            }

            // 5b. Hall Block Check
            var hallBlock = await _context.HallBlocks
                .Include(h => h.ConferenceRoom)
                .FirstOrDefaultAsync(h =>
                    h.ConferenceRoomId == booking.ConferenceRoomId &&
                    h.BlockedDate.Date == booking.StartTime.Date);

            if (hallBlock != null)
            {
                ModelState.AddModelError("", $"'{hallBlock.ConferenceRoom?.Name}' is unavailable on {hallBlock.BlockedDate:dd MMM yyyy} due to: {hallBlock.Reason} \u2013 {hallBlock.Notes}");
            }

            // 6. Handle file attachment upload
            if (attachment != null && attachment.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".png", ".jpg", ".jpeg" };
                var ext = Path.GetExtension(attachment.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("AttachmentPath", "Invalid file type. Allowed: PDF, Word, Excel, Images.");
                }
                else if (attachment.Length > 5 * 1024 * 1024) // 5 MB limit
                {
                    ModelState.AddModelError("AttachmentPath", "File size must be under 5 MB.");
                }
                else
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(attachment.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await attachment.CopyToAsync(stream);
                    booking.AttachmentPath = $"/uploads/{uniqueFileName}";
                }
            }

            // 7. Save to Database
            if (ModelState.IsValid)
            {
                if (itReq.NeedsVideoConferencing || itReq.NeedsProjector || itReq.NeedsWhiteboard ||
                    itReq.NeedsPASystem || itReq.NeedsLaptop || itReq.NeedsLaserPointer ||
                    itReq.MicCount > 0 || !string.IsNullOrEmpty(itReq.TechNotes))
                {
                    itReq.SetupStatus = TechSetupStatus.Pending;
                    booking.ITRequirement = itReq;
                }

                _context.Bookings.Add(booking);

                _context.AuditLogs.Add(new AuditLog
                {
                    UserId = userId,
                    UserName = User.Identity?.Name,
                    Action = "CREATE_BOOKING_REQUEST",
                    EntityName = "Booking",
                    Details = $"Requested room ID {booking.ConferenceRoomId} for {booking.StartTime:g} | Cost Centre: {booking.CostCentre}"
                });

                await _context.SaveChangesAsync();

                if (booking.Status == BookingStatus.Approved)
                {
                    TempData["SuccessMessage"] = $"Booking BKG-{booking.Id} submitted and automatically APPROVED.";
                }
                else
                {
                    TempData["SuccessMessage"] = $"Booking request BKG-{booking.Id} submitted successfully! Awaiting Allocator review.";
                }

                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns on validation failure
            ViewBag.Rooms = new SelectList(await _context.ConferenceRooms.Where(r => r.IsActive).ToListAsync(), "Id", "Name", booking.ConferenceRoomId);
            var activeCostCentres2 = await _context.CostCentres.Where(c => c.IsActive).OrderBy(c => c.Name).Select(c => new { Code = c.Code, Display = c.Code + " - " + c.Name }).ToListAsync();
            ViewBag.CostCentres = new SelectList(activeCostCentres2, "Code", "Display", booking.CostCentre);
            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int roomId, DateTime start, DateTime end, int? excludeBookingId)
        {
            if (start >= end) return Json(new { available = false, message = "Invalid time range." });

            var query = _context.Bookings.Where(b =>
                b.ConferenceRoomId == roomId &&
                b.Status != BookingStatus.Rejected &&
                b.Status != BookingStatus.Cancelled &&
                start < b.EndTime &&
                end > b.StartTime
            );

            if (excludeBookingId.HasValue)
                query = query.Where(b => b.Id != excludeBookingId.Value);

            var conflict = await query.Select(b => new { b.MeetingTitle, b.StartTime, b.EndTime }).FirstOrDefaultAsync();

            if (conflict != null)
            {
                return Json(new
                {
                    available = false,
                    message = $"Conflict with '{conflict.MeetingTitle}' ({conflict.StartTime:HH:mm} \u2013 {conflict.EndTime:HH:mm})."
                });
            }

            return Json(new { available = true, message = "Room is available!" });
        }

        // Returns all active rooms for a given date.
        // Blocked rooms are included but flagged with isBlocked=true and the reason,
        // so the client renders them disabled with a visible reason tooltip.
        [HttpGet]
        public async Task<IActionResult> GetAvailableRoomsForDate(string date)
        {
            if (!DateTime.TryParse(date, out var day))
                return Json(new List<object>());

            var rooms = await _context.ConferenceRooms
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();

            var blockedOnDate = await _context.HallBlocks
                .Where(h => h.BlockedDate.Date == day.Date)
                .ToListAsync();

            var result = rooms.Select(r =>
            {
                var block = blockedOnDate.FirstOrDefault(h => h.ConferenceRoomId == r.Id);
                return new
                {
                    id          = r.Id,
                    name        = r.Name,
                    isBlocked   = block != null,
                    blockReason = block != null ? $"Blocked \u2014 {block.Reason}: {block.Notes}" : (string?)null
                };
            });

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Calendar()
        {
            ViewBag.Rooms = await _context.ConferenceRooms.Where(r => r.IsActive).ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCalendarEvents(int? roomId)
        {
            var query = _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Where(b => b.Status != BookingStatus.Rejected && b.Status != BookingStatus.Cancelled);

            if (roomId.HasValue && roomId.Value > 0)
                query = query.Where(b => b.ConferenceRoomId == roomId.Value);

            var eventList = new List<object>();
            var bookings = await query.ToListAsync();

            foreach (var b in bookings)
            {
                eventList.Add(new
                {
                    id = b.Id,
                    title = $"{b.ConferenceRoom!.Name}: {b.MeetingTitle}",
                    start = b.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = b.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    color = b.Status == BookingStatus.Approved ? "#198754" : "#ffc107",
                    textColor = b.Status == BookingStatus.Approved ? "#ffffff" : "#000000",
                    extendedProps = new { status = b.Status.ToString(), department = b.Department, attendees = b.ExpectedAttendees }
                });
            }

            if (User.IsInRole("Admin") || User.IsInRole("Allocator"))
            {
                var groupedByDate = bookings.GroupBy(b => b.StartTime.Date);
                int totalRooms = roomId.HasValue && roomId.Value > 0 ? 1 : await _context.ConferenceRooms.CountAsync(r => r.IsActive);
                double totalCapacityHours = totalRooms * 8.0;

                foreach (var group in groupedByDate)
                {
                    double bookedHours = group.Sum(b => (b.EndTime - b.StartTime).TotalHours);

                    if (bookedHours >= totalCapacityHours)
                    {
                        eventList.Add(new
                        {
                            start = group.Key.ToString("yyyy-MM-dd"),
                            display = "background",
                            color = "rgba(220, 53, 69, 0.2)"
                        });
                    }
                    else if (bookedHours >= totalCapacityHours - 2.0)
                    {
                        eventList.Add(new
                        {
                            start = group.Key.ToString("yyyy-MM-dd"),
                            display = "background",
                            color = "rgba(253, 126, 20, 0.2)"
                        });
                    }
                }
            }

            return Json(eventList);
        }

        [HttpGet]
        public async Task<IActionResult> GetDaySlots(int roomId, string date)
        {
            if (!DateTime.TryParse(date, out var day)) return Json(new { error = "Invalid date" });

            // Check hall block
            var block = await _context.HallBlocks
                .Include(h => h.ConferenceRoom)
                .FirstOrDefaultAsync(h => h.ConferenceRoomId == roomId && h.BlockedDate.Date == day.Date);

            var room = await _context.ConferenceRooms.FindAsync(roomId);

            // Get bookings for this room on this day
            var dayStart = day.Date.AddHours(9).AddMinutes(30);
            var dayEnd = day.Date.AddHours(17).AddMinutes(30);

            var bookings = await _context.Bookings
                .Where(b => b.ConferenceRoomId == roomId &&
                            b.Status != BookingStatus.Rejected &&
                            b.Status != BookingStatus.Cancelled &&
                            b.StartTime < dayEnd && b.EndTime > dayStart)
                .ToListAsync();

            var slots = new List<object>();
            for (int h = 0; h < 8; h++)
            {
                var slotStart = dayStart.AddHours(h);
                var slotEnd = slotStart.AddHours(1);

                if (block != null)
                {
                    slots.Add(new {
                        hour = $"{slotStart:HH:mm} \u2013 {slotEnd:HH:mm}",
                        status = "Blocked",
                        details = $"Blocked: {block.Reason} ({block.Notes})",
                        isAvailable = false
                    });
                }
                else
                {
                    var match = bookings.FirstOrDefault(b => slotStart < b.EndTime && slotEnd > b.StartTime);
                    if (match != null)
                    {
                        slots.Add(new {
                            hour = $"{slotStart:HH:mm} \u2013 {slotEnd:HH:mm}",
                            status = match.Status == BookingStatus.Approved ? "Approved" : "Pending",
                            details = $"{match.MeetingTitle} ({match.OrganizerName})",
                            bookingId = match.Id,
                            isAvailable = false
                        });
                    }
                    else
                    {
                        slots.Add(new {
                            hour = $"{slotStart:HH:mm} \u2013 {slotEnd:HH:mm}",
                            status = "Available",
                            details = "Free Slot",
                            isAvailable = true,
                            slotStartStr = slotStart.ToString("yyyy-MM-ddTHH:mm")
                        });
                    }
                }
            }

            return Json(new {
                roomName = room?.Name ?? "Conference Hall",
                dateStr = day.ToString("dd MMMM yyyy"),
                isBlocked = block != null,
                blockReason = block?.Reason.ToString(),
                blockNotes = block?.Notes,
                slots
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (booking == null) return NotFound();

            if (booking.Status == BookingStatus.Pending || booking.Status == BookingStatus.PendingAllocatorReview)
            {
                booking.Status = BookingStatus.Cancelled;
                booking.RejectionReason = $"Cancelled by user {User.Identity?.Name}";

                _context.AuditLogs.Add(new AuditLog
                {
                    UserId = userId,
                    UserName = User.Identity?.Name,
                    Action = "CANCEL_BOOKING",
                    EntityName = "Booking",
                    EntityId = booking.Id.ToString(),
                    Details = $"Booking {booking.Id} cancelled by the user."
                });

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your booking request has been cancelled.";
            }
            else
            {
                TempData["ErrorMessage"] = "You can only cancel pending requests. For approved meetings, contact the Administrator.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
