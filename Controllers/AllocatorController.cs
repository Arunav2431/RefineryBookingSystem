// File: Controllers/AllocatorController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Data;
using RefineryBooking.Models;

namespace RefineryBooking.Controllers
{
    [Authorize(Roles = "Allocator,Admin")]
    public class AllocatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllocatorController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var pendingBookings = await _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.User)
                .Include(b => b.ITRequirement)
                .Where(b => b.Status == BookingStatus.Pending)
                .OrderBy(b => b.StartTime)
                .ToListAsync();

            return View(pendingBookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            // Final conflict verification before approval
            bool conflict = await _context.Bookings.AnyAsync(b =>
                b.Id != id &&
                b.ConferenceRoomId == booking.ConferenceRoomId &&
                b.Status == BookingStatus.Approved &&
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime);

            if (conflict)
            {
                TempData["ErrorMessage"] = "Cannot approve: A confirmed booking already occupies this time slot.";
                return RedirectToAction(nameof(Index));
            }

            booking.Status = BookingStatus.Approved;
            
            _context.AuditLogs.Add(new AuditLog {
                UserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name)?.Id,
                UserName = User.Identity?.Name,
                Action = "APPROVE_BOOKING",
                EntityName = "Booking",
                EntityId = id.ToString(),
                Details = $"Approved meeting '{booking.MeetingTitle}'"
            });

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Booking approved successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.Status = BookingStatus.Rejected;
            booking.RejectionReason = string.IsNullOrWhiteSpace(reason) ? "Not specified by Allocator." : reason;

            _context.AuditLogs.Add(new AuditLog {
                UserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name)?.Id,
                UserName = User.Identity?.Name,
                Action = "REJECT_BOOKING",
                EntityName = "Booking",
                EntityId = id.ToString(),
                Details = $"Rejected. Reason: {booking.RejectionReason}"
            });

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Booking request rejected.";
            return RedirectToAction(nameof(Index));
        }
    }
}