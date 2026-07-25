// File: Controllers/ITFMController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Data;
using RefineryBooking.Models;

namespace RefineryBooking.Controllers
{
    [Authorize(Roles = "ITFM,Admin")]
    public class ITFMController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ITFMController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var requirements = await _context.ITFacilityRequirements
                .Include(i => i.Booking)
                    .ThenInclude(b => b!.ConferenceRoom)
                .Include(i => i.Booking)
                    .ThenInclude(b => b!.User)
                .Where(i => i.Booking!.Status == BookingStatus.Approved && i.Booking.EndTime >= DateTime.Now)
                .OrderBy(i => i.Booking!.StartTime)
                .ToListAsync();

            return View(requirements);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, TechSetupStatus status)
        {
            var req = await _context.ITFacilityRequirements.FindAsync(id);
            if (req == null) return NotFound();

            req.SetupStatus = status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"AV/IT Setup status updated to {status}.";
            return RedirectToAction(nameof(Index));
        }
    }
}