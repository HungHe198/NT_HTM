using Microsoft.AspNetCore.Mvc;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System;

namespace NT.WEB.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly Services.IEmailService _emailService;

        public RegisterController(IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo, IPasswordHasher<User> passwordHasher, Services.IEmailService emailService)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(bool client = false)
        {
            ViewBag.IsClient = client;
            if (!client)
            {
                ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _roleRepo.GetAllAsync(), "Id", "Name");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User model, bool client = false)
        {
            // Server-side username format validation
            if (string.IsNullOrWhiteSpace(model.Username) || !System.Text.RegularExpressions.Regex.IsMatch(model.Username, "^[A-Za-z0-9._-]+$"))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập phải viết liền, không dấu, không chứa khoảng trắng; chỉ dùng chữ/số/./_/-.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.IsClient = client;
                if (!client) ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _roleRepo.GetAllAsync(), "Id", "Name");
                return View(model);
            }

            var exists = await _userRepo.FindAsync(u => u.Username == model.Username);
            if (System.Linq.Enumerable.Any(exists))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại");
                ViewBag.IsClient = client;
                if (!client) ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _roleRepo.GetAllAsync(), "Id", "Name");
                return View(model);
            }

            var plain = model.PasswordHash ?? string.Empty;
            model.PasswordHash = _passwordHasher.HashPassword(model, plain);

            // assign role
            if (model.RoleId == Guid.Empty || client)
            {
                var c = System.Linq.Enumerable.FirstOrDefault(await _roleRepo.FindAsync(r => r.Name == "Customer"));
                if (c != null) model.RoleId = c.Id;
            }

            await _userRepo.AddAsync(model);
            await _userRepo.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                try
                {
                    await _emailService.SendEmailAsync(model.Email, "đăng ký thành công", $"<p>Chào {System.Net.WebUtility.HtmlEncode(model.Fullname ?? model.Username)}</p><p>Bạn đã đăng ký thành công.</p>");
                }
                catch { }
            }

            TempData["Success"] = "đăng ký thành công.";
            return RedirectToAction("Index", "Login", new { client = client });
        }
    }
}
