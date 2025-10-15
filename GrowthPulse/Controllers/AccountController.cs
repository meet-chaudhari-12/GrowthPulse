using GrowthPulse.Models;
using GrowthPulse.Repositories;
using GrowthPulse.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GrowthPulse.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // --- REGISTRATION ACTIONS (already created) ---
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Email == viewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "A user with this email already exists.");
                    return View(viewModel);
                }

                UserRoles assignedRole;

                // For now, assign all new users as Customer
                assignedRole = UserRoles.Admin;
                
     
                var user = new User
                    {
                        FullName = viewModel.FullName,
                        Email = viewModel.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(viewModel.Password),
                        Role = assignedRole
                    };

                _unitOfWork.Users.Insert(user);
                _unitOfWork.Save();

                return RedirectToAction("Login");
            }
            return View(viewModel);
        }

        // --- LOGIN ACTIONS (newly added) --- 
        // GET: /Account/Login
        public IActionResult Login(string returnUrl = null)
        {
            var viewModel = new LoginViewModel { ReturnUrl = returnUrl };
            return View(viewModel);
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Email == viewModel.Email);

                // IMPORTANT: You MUST verify the hashed password here!
                // For example, using BCrypt.Net:
                // if (user != null && BCrypt.Net.BCrypt.Verify(viewModel.Password, user.PasswordHash))
                if (user != null && BCrypt.Net.BCrypt.Verify(viewModel.Password, user.PasswordHash))
                {
                    // Create the user's identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("FullName", user.FullName),
                        // You can add more claims here, like roles
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = viewModel.RememberMe
                    };

                    // Sign the user in
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
                        return Redirect(viewModel.ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(viewModel);
        }

        // --- LOGOUT ACTION (newly added) --- 🚪
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}