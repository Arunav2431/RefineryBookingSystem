using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Data;
using RefineryBooking.Models;

namespace RefineryBooking.Controllers
{
    [Authorize(Roles = "Admin,Allocator")]
    public class HallBlockController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public HallBlockController(ApplicationDbContext context,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ── LIST ALL BLOCKS ──────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var blocks = await _context.HallBlocks
                .Include(b => b.ConferenceRoom)
                .Include(b => b.CreatedBy)
                .Where(b => b.BlockedDate >= DateTime.Today)
                .OrderBy(b => b.BlockedDate)
                .ThenBy(b => b.ConferenceRoom!.Name)
                .ToListAsync();

            ViewBag.Rooms = await _context.ConferenceRooms
                .Where(r => r.IsActive).OrderBy(r => r.Name).ToListAsync();

            return View(blocks);
        }

        // ── CREATE GET ───────────────────────────────────────────────────────
        public async Task<IActionResult> Create()
        {
            ViewBag.Rooms = await _context.ConferenceRooms
                .Where(r => r.IsActive).OrderBy(r => r.Name).ToListAsync();
            return View();
        }

        // ── CREATE POST ──────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int conferenceRoomId, DateTime blockedDate,
            HallBlockReason reason, string? notes)
        {
            if (blockedDate < DateTime.Today)
            {
                TempData["ErrorMessage"] = "Cannot block a hall for a past date.";
                ViewBag.Rooms = await _context.ConferenceRooms
                    .Where(r => r.IsActive).OrderBy(r => r.Name).ToListAsync();
                return View();
            }

            // Check if already blocked
            var existing = await _context.HallBlocks
                .AnyAsync(b => b.ConferenceRoomId == conferenceRoomId &&
                               b.BlockedDate.Date == blockedDate.Date);
            if (existing)
            {
                TempData["ErrorMessage"] = "This hall is already blocked for the selected date.";
                ViewBag.Rooms = await _context.ConferenceRooms
                    .Where(r => r.IsActive).OrderBy(r => r.Name).ToListAsync();
                return View();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var block = new HallBlock
            {
                ConferenceRoomId = conferenceRoomId,
                BlockedDate = blockedDate.Date,
                Reason = reason,
                Notes = notes,
                CreatedByUserId = currentUser?.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.HallBlocks.Add(block);
            await _context.SaveChangesAsync();

            var room = await _context.ConferenceRooms.FindAsync(conferenceRoomId);
            TempData["SuccessMessage"] = $"'{room?.Name}' has been blocked on {blockedDate:dd MMM yyyy} ({reason}).";
            return RedirectToAction(nameof(Index));
        }

        // ── REMOVE POST ──────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var block = await _context.HallBlocks
                .Include(b => b.ConferenceRoom)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (block == null) return NotFound();

            _context.HallBlocks.Remove(block);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Hall block for '{block.ConferenceRoom?.Name}' on {block.BlockedDate:dd MMM yyyy} has been removed.";
            return RedirectToAction(nameof(Index));
        }

        // ── API: Get blocks as JSON for calendar ─────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetBlockedDates()
        {
            var blocks = await _context.HallBlocks
                .Include(b => b.ConferenceRoom)
                .Where(b => b.BlockedDate >= DateTime.Today.AddMonths(-1))
                .ToListAsync();

            var result = blocks.Select(b => new
            {
                date = b.BlockedDate.ToString("yyyy-MM-dd"),
                roomId = b.ConferenceRoomId,
                roomName = b.ConferenceRoom?.Name,
                reason = b.Reason.ToString(),
                notes = b.Notes
            });

            return Json(result);
        }
    }
}
