// File: Controllers/AccountController.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefineryBooking.Models;
using RefineryBooking.Services;

namespace RefineryBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyAuthService _companyAuth;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ICompanyAuthService companyAuth)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _companyAuth = companyAuth;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userId, string password, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and Password are required.");
                return View();
            }

            // ── STEP 1: Emergency Local Fallback (Strictly for System Admin) ─────
            // If the company server is down or credentials fail, we ONLY allow
            // the emergency 'sys.admin' account to authenticate locally.
            if (userId.Equals("sys.admin", StringComparison.OrdinalIgnoreCase))
            {
                var localResult = await _signInManager.PasswordSignInAsync(
                    userId, password, isPersistent: false, lockoutOnFailure: false);

                if (localResult.Succeeded)
                    return RedirectToLocal(returnUrl);
            }

            // ── STEP 2: Try company server authentication ────────────────────
            // In Strict AD Mode, all regular users MUST authenticate via the company server.
            var companyProfile = await _companyAuth.ValidateAndGetProfileAsync(userId, password);

            if (companyProfile != null)
            {
                // Find or create the user in the local DB
                var localUser = await _userManager.FindByNameAsync(userId);

                if (localUser == null)
                {
                    // ── FIRST LOGIN: Auto-provision the user ─────────────────
                    // Full Name and Department come from the company server profile.
                    localUser = new ApplicationUser
                    {
                        UserName        = userId,
                        Email           = companyProfile.Email,
                        FullName        = companyProfile.FullName,       // ← from company server
                        Department      = companyProfile.Department,     // ← from company server
                        EmployeeBadgeId = companyProfile.EmployeeId,
                        EmailConfirmed  = true
                    };

                    // Create user without a local password — auth is delegated to company server
                    var createResult = await _userManager.CreateAsync(localUser);
                    if (createResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(localUser, "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to provision account. Contact IT support.");
                        return View();
                    }
                }
                else
                {
                    // ── SUBSEQUENT LOGIN: Refresh profile from company server ──
                    // Keeps Full Name and Department in sync with company directory.
                    localUser.FullName   = companyProfile.FullName;
                    localUser.Department = companyProfile.Department;
                    if (!string.IsNullOrEmpty(companyProfile.Email))
                        localUser.Email = companyProfile.Email;
                    await _userManager.UpdateAsync(localUser);
                }

                await _signInManager.SignInAsync(localUser, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }

            // If both fail:
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
    }
}