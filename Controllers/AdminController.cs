using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Data;
using RefineryBooking.Models;
using System.Security.Claims;

namespace RefineryBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ── USER LIST ────────────────────────────────────────────────────────
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.OrderBy(u => u.FullName).ToListAsync();
            var userRoles = new Dictionary<string, string>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles.FirstOrDefault() ?? "—";
            }
            ViewBag.UserRoles = userRoles;
            return View("UserList", users);
        }

        // ── CREATE USER GET ──────────────────────────────────────────────────
        public IActionResult CreateUser()
        {
            ViewBag.Roles = new List<string> { "User", "Allocator", "ITFM", "Admin" };
            ViewBag.Departments = GetDepartments();
            return View();
        }

        // ── CREATE USER POST ─────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(string fullName, string email,
            string employeeBadgeId, string department, string role, string password)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                TempData["ErrorMessage"] = "All required fields must be filled.";
                ViewBag.Roles = new List<string> { "User", "Allocator", "ITFM", "Admin" };
                ViewBag.Departments = GetDepartments();
                return View();
            }

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = $"A user with email '{email}' already exists.";
                ViewBag.Roles = new List<string> { "User", "Allocator", "ITFM", "Admin" };
                ViewBag.Departments = GetDepartments();
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmployeeBadgeId = employeeBadgeId,
                Department = department,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Error: " + string.Join(", ", result.Errors.Select(e => e.Description));
                ViewBag.Roles = new List<string> { "User", "Allocator", "ITFM", "Admin" };
                ViewBag.Departments = GetDepartments();
                return View();
            }

            await _userManager.AddToRoleAsync(user, role);
            TempData["SuccessMessage"] = $"User '{fullName}' ({email}) created successfully with role '{role}'.";
            return RedirectToAction(nameof(Users));
        }

        // ── EDIT ROLE POST ───────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);
            await _userManager.AddToRoleAsync(user, newRole);

            TempData["SuccessMessage"] = $"Role for '{user.FullName}' updated to '{newRole}'.";
            return RedirectToAction(nameof(Users));
        }

        // ── DEACTIVATE USER POST ─────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;
            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = $"User '{user.FullName}' has been deactivated.";
            return RedirectToAction(nameof(Users));
        }

        // ── REACTIVATE USER POST ─────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ReactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            await _userManager.SetLockoutEndDateAsync(user, null);
            TempData["SuccessMessage"] = $"User '{user.FullName}' has been reactivated.";
            return RedirectToAction(nameof(Users));
        }

        // ── BOOKING HISTORY (ADMIN ONLY) ─────────────────────────────────────
        public async Task<IActionResult> BookingHistory(string? searchHallName, int page = 1)
        {
            int pageSize = 50;
            var query = _context.Bookings
                .Include(b => b.ConferenceRoom)
                .Include(b => b.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchHallName))
            {
                query = query.Where(b => b.ConferenceRoom != null && b.ConferenceRoom.Name.Contains(searchHallName));
            }

            var bookings = await query
                .OrderBy(b => b.Status == BookingStatus.Pending || b.Status == BookingStatus.PendingAllocatorReview ? 0 : 1)
                .ThenByDescending(b => b.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.SearchHallName = searchHallName;
            ViewBag.CurrentPage = page;
            ViewBag.HasNextPage = bookings.Count == pageSize; 
            
            return View(bookings);
        }

        // ── CANCEL BOOKING (ADMIN ONLY) ──────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            if (booking.EndTime > DateTime.Now && booking.Status != BookingStatus.Cancelled && booking.Status != BookingStatus.Rejected)
            {
                booking.Status = BookingStatus.Cancelled;
                booking.RejectionReason = $"Cancelled by Admin ({User.Identity?.Name})";

                _context.AuditLogs.Add(new AuditLog
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                    UserName = User.Identity?.Name,
                    Action = "CANCEL_BOOKING_ADMIN",
                    EntityName = "Booking",
                    EntityId = booking.Id.ToString(),
                    Details = $"Booking {booking.Id} cancelled by Admin."
                });

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Booking BKG-{booking.Id} has been cancelled.";
            }
            else
            {
                TempData["ErrorMessage"] = "This booking cannot be cancelled (it may have already completed, or is already cancelled).";
            }

            return RedirectToAction(nameof(BookingHistory));
        }

        private static List<string> GetDepartments() => new()
        {
            "Operations", "Engineering", "Maintenance", "HSE",
            "IT", "Finance", "HR", "Logistics", "Quality Control",
            "Admin & Corporate Affairs", "Security", "Planning"
        };
    }
}
