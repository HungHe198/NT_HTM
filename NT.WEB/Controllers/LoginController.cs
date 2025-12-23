using Microsoft.AspNetCore.Mvc;
using NT.BLL.Interfaces;
using NT.SHARED.DTOs;
using NT.SHARED.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System;

namespace NT.WEB.Controllers
{
    public class LoginController : Controller
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginController(IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo, IPasswordHasher<User> passwordHasher)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index(bool client = false)
        {
            // No dedicated Login view under Views/Login.
            // Redirect to the existing Account/Login view to avoid InvalidOperationException.
            return RedirectToAction("Login", "Account", new { msg = (string?)null });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginDto dto, bool client = false)
        {
            if (!ModelState.IsValid)
            {
                // Redirect to the shared Account/Login view if model invalid to keep a single view location
                return RedirectToAction("Login", "Account");
            }

            var users = await _userRepo.FindAsync(u => u.Username == dto.Username);
            var user = System.Linq.Enumerable.FirstOrDefault(users);
            if (user == null)
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng";
                return RedirectToAction("Login", "Account");
            }

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verify != PasswordVerificationResult.Success)
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng";
                return RedirectToAction("Login", "Account");
            }

            // If user status indicates disabled, block login
            var isActive = (user.Status == "1") || string.Equals(user.Status, "Active", StringComparison.OrdinalIgnoreCase);
            if (!isActive)
            {
                TempData["Error"] = "Tài khoản này đã bị vô hiệu hóa. Vui lòng liên hệ quản trị viên.";
                return RedirectToAction("Login", "Account");
            }

            // get role name
            string roleName = "Customer";
            if (user.RoleId != Guid.Empty)
            {
                var role = await _roleRepo.GetByIdAsync(user.RoleId);
                if (role != null) roleName = role.Name;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, roleName)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            TempData["Success"] = "Đăng nhập thành công.";

            var rn = roleName?.ToLowerInvariant();
            if (rn == "admin" || rn == "employee")
                return RedirectToAction("Index", "Admin");

            // For customers redirect to home (client area)
            return RedirectToAction("Index", "Home");
        }
    }
}
