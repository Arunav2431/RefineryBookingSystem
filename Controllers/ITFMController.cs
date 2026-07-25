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

        // ── INDEX: All approved upcoming bookings that have IT requirements
        //           OR explicitly requested ITFM help ─────────────────────────
        public async Task<IActionResult> Index(string tab = "setup")
        {
            ViewBag.ActiveTab = tab;

            // Bookings requesting ITFM help OR with IT facility requirements
            var baseQuery = _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.User)
                .Include(b => b.ITRequirement)
                .AsQueryable();

            IQueryable<Booking> filtered = tab switch
            {
                "helpdesk" => baseQuery.Where(b =>
                    b.RequiresITFMHelp &&
                    (b.Status == BookingStatus.Pending || b.Status == BookingStatus.PendingAllocatorReview || b.Status == BookingStatus.Approved)),
                _ => baseQuery.Where(b =>
                    b.Status == BookingStatus.Approved &&
                    b.EndTime >= DateTime.Now &&
                    b.ITRequirement != null)
            };

            var bookings = await filtered.OrderBy(b => b.StartTime).ToListAsync();

            ViewBag.CountSetup    = await _context.Bookings.CountAsync(b =>
                b.Status == BookingStatus.Approved && b.EndTime >= DateTime.Now && b.ITRequirement != null);
            ViewBag.CountHelpdesk = await _context.Bookings.CountAsync(b =>
                b.RequiresITFMHelp &&
                (b.Status == BookingStatus.Pending || b.Status == BookingStatus.PendingAllocatorReview || b.Status == BookingStatus.Approved));

            return View(bookings);
        }

        // ── DETAILS: Full booking details + all equipment ────────────────────
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.User)
                .Include(b => b.ITRequirement)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();
            return View(booking);
        }

        // ── UPDATE IT SETUP STATUS ───────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, TechSetupStatus status)
        {
            var req = await _context.ITFacilityRequirements.FindAsync(id);
            if (req == null) return NotFound();

            req.SetupStatus = status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Setup status updated to: {status}.";
            return RedirectToAction(nameof(Index));
        }

        // ── ADD TECH NOTE ────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(int itReqId, string note)
        {
            var req = await _context.ITFacilityRequirements.FindAsync(itReqId);
            if (req == null) return NotFound();

            req.TechNotes = note;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Technical note saved.";
            return RedirectToAction(nameof(Details), new { id = req.BookingId });
        }
    }
}