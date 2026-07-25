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

        private static readonly List<string> CostCentres = new()
        {
            "CCU-001 – Catalytic Cracking Unit",
            "CCU-002 – Fluid Catalytic Cracker (FCC)",
            "REF-010 – Crude Distillation Unit (CDU)",
            "REF-011 – Vacuum Distillation Unit (VDU)",
            "REF-012 – Hydrocracker Unit",
            "REF-013 – Naphtha Hydrotreater",
            "REF-014 – Reformer Unit (CCR)",
            "REF-015 – Alkylation Unit",
            "REF-016 – Isomerisation Unit",
            "PLN-020 – Pipeline & Distribution",
            "PLN-021 – Product Storage & Tankage",
            "PLN-022 – Offsites & Utilities",
            "HSE-030 – Health, Safety & Environment",
            "HSE-031 – HAZMAT Response Team",
            "HSE-032 – Environmental Compliance",
            "HSE-033 – Process Safety Management",
            "ENG-040 – Mechanical Engineering",
            "ENG-041 – Electrical Engineering",
            "ENG-042 – Instrumentation & Control",
            "ENG-043 – Civil & Structural Engineering",
            "ENG-044 – Rotating Equipment",
            "ENG-045 – Static Equipment & Piping",
            "MAINT-050 – Maintenance Planning",
            "MAINT-051 – Turnaround Management",
            "MAINT-052 – Shutdown & Start-up",
            "IT-060 – Information Technology",
            "IT-061 – DCS / SCADA / OT Systems",
            "IT-062 – Cybersecurity & Network",
            "FIN-070 – Finance & Accounting",
            "FIN-071 – Procurement & Contracts",
            "FIN-072 – Budget & Cost Control",
            "LOG-080 – Logistics & Supply Chain",
            "LOG-081 – Crude Receipt & Scheduling",
            "LOG-082 – Product Dispatch",
            "HR-090 – Human Resources",
            "HR-091 – Training & Development",
            "HR-092 – Workforce Planning",
            "ADM-100 – Administration & Corporate Affairs",
            "ADM-101 – Legal & Compliance",
            "ADM-102 – Communications & PR",
            "SEC-110 – Security & Access Control",
            "QC-120 – Quality Control & Laboratory",
            "QC-121 – Product Quality Assurance",
            "OPS-130 – Operations Management",
            "OPS-131 – Production Planning",
            "OPS-132 – Plant Optimisation",
        };

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
            ViewBag.CostCentres = new SelectList(CostCentres);

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
            booking.Status = BookingStatus.Pending;
            booking.CreatedAt = DateTime.UtcNow;

            // 2. Clear validation checks for properties set programmatically
            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("ConferenceRoom");
            ModelState.Remove("ITRequirement");
            ModelState.Remove("itReq.Booking");
            ModelState.Remove("itReq.BookingId");

            // 3. Working Hours Validation (08:00 – 18:00)
            var workStart = booking.StartTime.Date.AddHours(8);
            var workEnd = booking.StartTime.Date.AddHours(18);
            if (booking.StartTime < workStart || booking.EndTime > workEnd)
            {
                ModelState.AddModelError("", "Bookings must be within working hours: 08:00 – 18:00.");
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
                TempData["SuccessMessage"] = $"Booking request BKG-{booking.Id} submitted successfully! Awaiting Allocator review.";
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns on validation failure
            ViewBag.Rooms = new SelectList(await _context.ConferenceRooms.Where(r => r.IsActive).ToListAsync(), "Id", "Name", booking.ConferenceRoomId);
            ViewBag.CostCentres = new SelectList(CostCentres, booking.CostCentre);
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
                    message = $"Conflict with '{conflict.MeetingTitle}' ({conflict.StartTime:HH:mm} – {conflict.EndTime:HH:mm})."
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
                query = query.Where(b => b.ConferenceRoomId == roomId.Value);

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