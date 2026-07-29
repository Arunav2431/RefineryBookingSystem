using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Data;
using RefineryBooking.Models;
using RefineryBooking.Services;
using System.Security.Claims;

namespace RefineryBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICompanyAuthService _companyAuth;

        public AdminController(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               ICompanyAuthService companyAuth)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _companyAuth = companyAuth;
        }

        // â”€â”€ USER LIST â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.OrderBy(u => u.FullName).ToListAsync();
            var userRoles = new Dictionary<string, string>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles.FirstOrDefault() ?? "â€”";
            }
            ViewBag.UserRoles = userRoles;

            // Hall assignment counts for each Allocator (shown in UserList)
            var hallCounts = await _context.AllocatorHallAssignments
                .GroupBy(a => a.AllocatorUserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.UserId, x => x.Count);
            ViewBag.HallCounts = hallCounts;

            return View("UserList", users);
        }

        // â”€â”€ CREATE USER GET â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Roles    = new List<string> { "Allocator", "ITFM", "Admin" };
            ViewBag.AllRooms = await _context.ConferenceRooms
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
            return View();
        }

        // â”€â”€ CREATE USER POST â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(string windowsUsername,
            string employeeBadgeId, string role, string password, int[]? selectedRoomIds)
        {
            var allowedRoles = new[] { "Admin", "ITFM", "Allocator" };
            selectedRoomIds ??= Array.Empty<int>();

            async Task<IActionResult> ReturnWithError(string msg)
            {
                TempData["ErrorMessage"] = msg;
                ViewBag.Roles    = new List<string> { "Allocator", "ITFM", "Admin" };
                ViewBag.AllRooms = await _context.ConferenceRooms
                    .Where(r => r.IsActive).OrderBy(r => r.Name).ToListAsync();
                return View();
            }

            if (string.IsNullOrWhiteSpace(windowsUsername) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role) ||
                !allowedRoles.Contains(role))
                return await ReturnWithError("Windows Username, Password, and a valid Role are required.");

            // Allocator MUST have at least one hall assigned at creation
            if (role == "Allocator" && selectedRoomIds.Length == 0)
                return await ReturnWithError("An Allocator must be assigned to at least one conference hall.");

            var existingUser = await _userManager.FindByNameAsync(windowsUsername);
            if (existingUser != null)
                return await ReturnWithError($"A user with username '{windowsUsername}' already exists.");

            // â”€â”€ Fetch Full Name & Department from company server â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
            var companyProfile = await _companyAuth.GetProfileAsync(windowsUsername);
            var fullName   = companyProfile?.FullName   ?? $"({windowsUsername} â€” pending company sync)";
            var department = companyProfile?.Department ?? "(Pending company sync)";
            var email      = companyProfile?.Email      ?? $"{windowsUsername}@nrl.co.in";

            var user = new ApplicationUser
            {
                UserName        = windowsUsername,
                Email           = email,
                FullName        = fullName,
                Department      = department,
                EmployeeBadgeId = employeeBadgeId,
                EmailConfirmed  = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return await ReturnWithError("Error: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, role);

            // â”€â”€ Save hall assignments for Allocator â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
            if (role == "Allocator")
            {
                var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                foreach (var roomId in selectedRoomIds)
                {
                    _context.AllocatorHallAssignments.Add(new AllocatorHallAssignment
                    {
                        AllocatorUserId  = user.Id,
                        ConferenceRoomId = roomId,
                        AssignedAt       = DateTime.UtcNow,
                        AssignedByUserId = adminId
                    });
                }
                await _context.SaveChangesAsync();
            }

            var profileNote = companyProfile != null
                ? $"Name '{fullName}', Dept '{department}' fetched from company server."
                : "Company server not yet connected â€” name/dept will update on first login.";

            var hallNote = role == "Allocator"
                ? $" Assigned to {selectedRoomIds.Length} hall(s)."
                : string.Empty;

            TempData["SuccessMessage"] = $"Account '{windowsUsername}' created as {role}. {profileNote}{hallNote}";
            return RedirectToAction(nameof(Users));
        }

        // â”€â”€ EDIT ROLE POST â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
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

        // â”€â”€ DEACTIVATE USER POST â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
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

        // â”€â”€ REACTIVATE USER POST â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ReactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            await _userManager.SetLockoutEndDateAsync(user, null);
            TempData["SuccessMessage"] = $"User '{user.FullName}' has been reactivated.";
            return RedirectToAction(nameof(Users));
        }

        // â”€â”€ BOOKING HISTORY (ADMIN ONLY) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
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

        // â”€â”€ CANCEL BOOKING (ADMIN ONLY) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
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

        // â”€â”€ MANAGE ALLOCATOR HALL ASSIGNMENTS GET â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public async Task<IActionResult> ManageAllocatorHalls(string userId)
        {
            var allocator = await _userManager.FindByIdAsync(userId);
            if (allocator == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(allocator);
            if (!roles.Contains("Allocator"))
            {
                TempData["ErrorMessage"] = "Hall assignments are only applicable to Allocator role users.";
                return RedirectToAction(nameof(Users));
            }

            // All conference rooms
            var allRooms = await _context.ConferenceRooms
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();

            // Rooms currently assigned to this allocator
            var assignedRoomIds = await _context.AllocatorHallAssignments
                .Where(a => a.AllocatorUserId == userId)
                .Select(a => a.ConferenceRoomId)
                .ToListAsync();

            ViewBag.Allocator = allocator;
            ViewBag.AssignedRoomIds = assignedRoomIds;
            return View(allRooms);
        }

        // â”€â”€ MANAGE ALLOCATOR HALL ASSIGNMENTS POST â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageAllocatorHalls(string userId, int[]? selectedRoomIds)
        {
            var allocator = await _userManager.FindByIdAsync(userId);
            if (allocator == null) return NotFound();

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            selectedRoomIds ??= Array.Empty<int>();

            // Remove all existing assignments for this allocator
            var existing = await _context.AllocatorHallAssignments
                .Where(a => a.AllocatorUserId == userId)
                .ToListAsync();
            _context.AllocatorHallAssignments.RemoveRange(existing);

            // Add the newly selected ones
            foreach (var roomId in selectedRoomIds)
            {
                _context.AllocatorHallAssignments.Add(new AllocatorHallAssignment
                {
                    AllocatorUserId  = userId,
                    ConferenceRoomId = roomId,
                    AssignedAt       = DateTime.UtcNow,
                    AssignedByUserId = adminId
                });
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = selectedRoomIds.Length == 0
                ? $"All hall assignments removed for '{allocator.FullName}'."
                : $"'{allocator.FullName}' is now assigned to {selectedRoomIds.Length} hall(s).";

            return RedirectToAction(nameof(Users));
        }

        // â”€â”€ MANAGE HALLS (ADMIN) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        public async Task<IActionResult> Halls()
        {
            var halls = await _context.ConferenceRooms.OrderBy(h => h.Name).ToListAsync();
            return View(halls);
        }

        public IActionResult AddHall()
        {
            ViewBag.Departments = GetDepartments();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHall(ConferenceRoom hall)
        {
            ViewBag.Departments = GetDepartments();

            if (!ModelState.IsValid)
                return View(hall);

            // Validate CostCentre is numeric
            if (!int.TryParse(hall.CostCentreCode, out _))
            {
                ModelState.AddModelError("CostCentreCode", "Cost Centre Code must be numeric.");
                return View(hall);
            }
            hall.CostCentreCode = hall.CostCentreCode.PadLeft(4, '0');

            // Generate HallCode if empty
            if (string.IsNullOrWhiteSpace(hall.HallCode))
            {
                string deptPrefix = hall.OwnerDepartment.Length >= 3 ? hall.OwnerDepartment.Substring(0, 3).ToUpper() : "GEN";
                int nextSeq = await _context.ConferenceRooms
                    .Where(r => r.CostCentreCode == hall.CostCentreCode && r.OwnerDepartment == hall.OwnerDepartment)
                    .CountAsync() + 1;
                hall.HallCode = $"CC-{hall.CostCentreCode}-{deptPrefix}-{nextSeq:D2}";
            }

            // Ensure HallCode is unique
            if (await _context.ConferenceRooms.AnyAsync(r => r.HallCode == hall.HallCode))
            {
                ModelState.AddModelError("HallCode", "This Hall Code already exists. Please modify it.");
                return View(hall);
            }

            // Ensure Name is unique
            if (await _context.ConferenceRooms.AnyAsync(r => r.Name.Trim().ToLower() == hall.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "A hall with this name already exists.");
                return View(hall);
            }

            hall.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            hall.CreatedAt = DateTime.UtcNow;
            
            _context.ConferenceRooms.Add(hall);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Hall '{hall.Name}' ({hall.HallCode}) added successfully.";
            return RedirectToAction(nameof(Halls));
        }

        public async Task<IActionResult> EditHall(int id)
        {
            var hall = await _context.ConferenceRooms.FindAsync(id);
            if (hall == null) return NotFound();
            
            ViewBag.Departments = GetDepartments();
            return View(hall);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHall(int id, ConferenceRoom model)
        {
            if (id != model.Id) return NotFound();
            ViewBag.Departments = GetDepartments();

            if (!ModelState.IsValid)
                return View(model);

            if (!int.TryParse(model.CostCentreCode, out _))
            {
                ModelState.AddModelError("CostCentreCode", "Cost Centre Code must be numeric.");
                return View(model);
            }
            model.CostCentreCode = model.CostCentreCode.PadLeft(4, '0');

            if (await _context.ConferenceRooms.AnyAsync(r => r.Id != model.Id && r.HallCode == model.HallCode))
            {
                ModelState.AddModelError("HallCode", "This Hall Code already exists for another room.");
                return View(model);
            }

            if (await _context.ConferenceRooms.AnyAsync(r => r.Id != model.Id && r.Name.Trim().ToLower() == model.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "A hall with this name already exists.");
                return View(model);
            }

            var hall = await _context.ConferenceRooms.FindAsync(id);
            if (hall == null) return NotFound();

            hall.Name = model.Name;
            hall.HallCode = model.HallCode;
            hall.OwnerDepartment = model.OwnerDepartment;
            hall.CostCentreCode = model.CostCentreCode;
            hall.BuildingLocation = model.BuildingLocation;
            hall.FloorNumber = model.FloorNumber;
            hall.Capacity = model.Capacity;
            hall.Description = model.Description;
            hall.HasProjector = model.HasProjector;
            hall.HasVideoConferencing = model.HasVideoConferencing;
            hall.HasWhiteboard = model.HasWhiteboard;

            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Hall '{hall.Name}' updated successfully.";
            return RedirectToAction(nameof(Halls));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleHall(int id)
        {
            var hall = await _context.ConferenceRooms.FindAsync(id);
            if (hall == null) return NotFound();

            if (hall.IsActive)
            {
                // Check for upcoming approved bookings
                var upcomingBookings = await _context.Bookings
                    .Where(b => b.ConferenceRoomId == id && b.Status == BookingStatus.Approved && b.EndTime > DateTime.Now)
                    .CountAsync();

                if (upcomingBookings > 0)
                {
                    TempData["ErrorMessage"] = $"Cannot deactivate hall '{hall.Name}' because there are {upcomingBookings} upcoming approved booking(s). Please cancel them first or let them complete.";
                    return RedirectToAction(nameof(Halls));
                }
            }

            hall.IsActive = !hall.IsActive;
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Hall '{hall.Name}' is now {(hall.IsActive ? "Active" : "Inactive")}.";
            return RedirectToAction(nameof(Halls));
        }

        private static List<string> GetDepartments() => new()
        {
            "Operations", "Engineering", "Maintenance", "HSE",
            "IT", "Finance", "HR", "Logistics", "Quality Control",
            "Admin & Corporate Affairs", "Security", "Planning"
        };
    }
}

