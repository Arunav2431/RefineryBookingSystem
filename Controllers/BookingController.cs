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

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            
            var model = new Booking();
            if (roomId.HasValue && !string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var parsedDate))
            {
                model.ConferenceRoomId = roomId.Value;
                model.StartTime = parsedDate.AddHours(9); // Default 9 AM
                model.EndTime = parsedDate.AddHours(10);  // Default 10 AM
            }
            else
            {
                model.StartTime = DateTime.Today.AddDays(1).AddHours(9);
                model.EndTime = DateTime.Today.AddDays(1).AddHours(10);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null) model.Department = user.Department;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking, ITFacilityRequirement itReq)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            // 1. Assign backend data immediately
            booking.UserId = userId;
            booking.Status = BookingStatus.Pending;
            booking.CreatedAt = DateTime.UtcNow;

            // 2. Clear validation checks for properties set programmatically in the backend
            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("ConferenceRoom");
            ModelState.Remove("ITRequirement");
            ModelState.Remove("itReq.Booking");
            ModelState.Remove("itReq.BookingId");

            // 3. Real-time Database Conflict Check
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

            if (booking.StartTime >= booking.EndTime)
            {
                ModelState.AddModelError("EndTime", "End time must be after start time.");
            }

            if (booking.StartTime < DateTime.Now)
            {
                ModelState.AddModelError("StartTime", "Cannot book meetings in the past.");
            }

            // 4. Save to Database!
            if (ModelState.IsValid)
            {
                if (itReq.NeedsVideoConferencing || itReq.NeedsProjector || itReq.MicCount > 0 || !string.IsNullOrEmpty(itReq.TechNotes))
                {
                    itReq.SetupStatus = TechSetupStatus.Pending;
                    booking.ITRequirement = itReq;
                }

                _context.Bookings.Add(booking);

                // Write to Audit Log
                _context.AuditLogs.Add(new AuditLog
                {
                    UserId = userId,
                    UserName = User.Identity?.Name,
                    Action = "CREATE_BOOKING_REQUEST",
                    EntityName = "Booking",
                    Details = $"Requested room ID {booking.ConferenceRoomId} for {booking.StartTime:g}"
                });

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Room booking request submitted successfully! Awaiting Allocator review.";
                return RedirectToAction(nameof(Index));
            }

            // If validation failed, reload the room dropdown so the form doesn't crash
            ViewBag.Rooms = new SelectList(await _context.ConferenceRooms.Where(r => r.IsActive).ToListAsync(), "Id", "Name", booking.ConferenceRoomId);
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
            {
                query = query.Where(b => b.Id != excludeBookingId.Value);
            }

            var conflict = await query.Select(b => new { b.MeetingTitle, b.StartTime, b.EndTime }).FirstOrDefaultAsync();

            if (conflict != null)
            {
                return Json(new { 
                    available = false, 
                    message = $"Conflict with '{conflict.MeetingTitle}' ({conflict.StartTime:HH:mm} - {conflict.EndTime:HH:mm})." 
                });
            }

            return Json(new { available = true, message = "Room is available!" });
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
            {
                query = query.Where(b => b.ConferenceRoomId == roomId.Value);
            }

            var events = await query.Select(b => new
            {
                id = b.Id,
                title = $"{b.ConferenceRoom!.Name}: {b.MeetingTitle}",
                start = b.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                end = b.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                color = b.Status == BookingStatus.Approved ? "#198754" : "#ffc107",
                textColor = b.Status == BookingStatus.Approved ? "#ffffff" : "#000000",
                extendedProps = new { status = b.Status.ToString(), department = b.Department, attendees = b.ExpectedAttendees }
            }).ToListAsync();

            return Json(events);
        }
    }
}