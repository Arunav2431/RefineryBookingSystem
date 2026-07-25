// File: Controllers/HomeController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Data;
using RefineryBooking.Models;
using System.Security.Claims;

namespace RefineryBooking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // --- 1. ADMIN METRICS ---
            if (User.IsInRole("Admin"))
            {
                ViewBag.TotalUsers = await _context.Users.CountAsync();
                ViewBag.TotalRooms = await _context.ConferenceRooms.CountAsync();
                ViewBag.TotalBookings = await _context.Bookings.CountAsync();
                ViewBag.RecentLogs = await _context.AuditLogs
                    .OrderByDescending(a => a.Timestamp)
                    .Take(5)
                    .ToListAsync();
            }

            // --- 2. ALLOCATOR METRICS ---
            if (User.IsInRole("Allocator"))
            {
                ViewBag.PendingCount = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Pending);
                ViewBag.TodayCount = await _context.Bookings.CountAsync(b => b.StartTime.Date == DateTime.Today && b.Status == BookingStatus.Approved);
                ViewBag.PendingList = await _context.Bookings
                    .Include(b => b.ConferenceRoom)
                    .Include(b => b.User)
                    .Where(b => b.Status == BookingStatus.Pending)
                    .OrderBy(b => b.StartTime)
                    .Take(5)
                    .ToListAsync();
            }

            // --- 3. ITFM / FACILITIES METRICS ---
            if (User.IsInRole("ITFM"))
            {
                ViewBag.PendingTechCount = await _context.ITFacilityRequirements
                    .CountAsync(i => i.SetupStatus == TechSetupStatus.Pending && i.Booking!.Status == BookingStatus.Approved);
                ViewBag.TodayCateringCount = await _context.Bookings
                    .CountAsync(b => b.StartTime.Date == DateTime.Today && b.Status == BookingStatus.Approved && b.RequiresCatering);
                ViewBag.UpcomingTechList = await _context.ITFacilityRequirements
                    .Include(i => i.Booking)
                        .ThenInclude(b => b!.ConferenceRoom)
                    .Include(i => i.Booking)
                        .ThenInclude(b => b!.User)
                    .Where(i => i.SetupStatus == TechSetupStatus.Pending && i.Booking!.Status == BookingStatus.Approved)
                    .OrderBy(i => i.Booking!.StartTime)
                    .Take(5)
                    .ToListAsync();
            }

            // --- 4. GENERAL USER METRICS ---
            ViewBag.MyApprovedCount = await _context.Bookings.CountAsync(b => b.UserId == userId && b.Status == BookingStatus.Approved && b.EndTime >= DateTime.Now);
            ViewBag.MyPendingCount = await _context.Bookings.CountAsync(b => b.UserId == userId && b.Status == BookingStatus.Pending);

            var myBookings = await _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.ITRequirement)
                .Where(b => b.UserId == userId && b.EndTime >= DateTime.Now)
                .OrderBy(b => b.StartTime)
                .Take(5)
                .ToListAsync();

            return View(myBookings);
        }

        [AllowAnonymous]
        public IActionResult Helpdesk() => View();
    }
}