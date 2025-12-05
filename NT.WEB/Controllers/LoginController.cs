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
            ViewBag.IsClient = client;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginDto dto, bool client = false)
        {
            if (!ModelState.IsValid) return View(dto);

            var users = await _userRepo.FindAsync(u => u.Username == dto.Username);
            var user = System.Linq.Enumerable.FirstOrDefault(users);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên ??ng nh?p ho?c m?t kh?u không ?úng");
                return View(dto);
            }

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verify != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError(string.Empty, "Tên ??ng nh?p ho?c m?t kh?u không ?úng");
                return View(dto);
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

            TempData["Success"] = "??ng nh?p thành công.";

            var rn = roleName?.ToLowerInvariant();
            if (rn == "admin" || rn == "employee")
                return RedirectToAction("Index", "Admin");

            // For customers redirect to home (client area)
            return RedirectToAction("Index", "Home");
        }
    }
}
