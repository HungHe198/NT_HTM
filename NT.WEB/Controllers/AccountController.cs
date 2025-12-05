using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using NT.SHARED.Models;

namespace NT.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IGenericRepository<NT.SHARED.Models.Role> _roleRepository;
        private readonly Microsoft.AspNetCore.Identity.IPasswordHasher<User> _passwordHasher;
        private readonly Services.IEmailService _emailService;

        public AccountController(IGenericRepository<User> repository, Microsoft.AspNetCore.Identity.IPasswordHasher<User> passwordHasher, Services.IEmailService emailService, IGenericRepository<NT.SHARED.Models.Role> roleRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<IActionResult> Index()
        {
            var items = await _repository.GetAllAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // Registration (GET)
        // If client==true then this request comes from client site and we won't show role selector
        public async Task<IActionResult> Create(bool client = false)
        {
            ViewBag.IsClient = client;
            if (!client)
            {
                ViewBag.Roles = new SelectList(await _roleRepository.GetAllAsync(), "Id", "Name");
            }
            return View();
        }

        // Registration (POST) - hash password, save and send confirmation email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check existing username
            var existing = await _repository.FindAsync(u => u.Username == model.Username);
            if (existing is not null && System.Linq.Enumerable.Any(existing))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại");
                return View(model);
            }

            // Hash password (assume model.PasswordHash currently contains plain password)
            var plain = model.PasswordHash ?? string.Empty;
            model.PasswordHash = _passwordHasher.HashPassword(model, plain);

            // If RoleId not specified, set to Customer role by name
            if (model.RoleId == Guid.Empty)
            {
                var customers = await _roleRepository.FindAsync(r => r.Name == "Customer");
                var role = System.Linq.Enumerable.FirstOrDefault(customers);
                if (role != null)
                {
                    model.RoleId = role.Id;
                }
                else
                {
                    var roles = await _roleRepository.GetAllAsync();
                    var first = System.Linq.Enumerable.FirstOrDefault(roles);
                    if (first != null) model.RoleId = first.Id;
                }
            }

            await _repository.AddAsync(model);
            await _repository.SaveChangesAsync();

            // Send confirmation email if email provided
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                try
                {
                    var subject = "Đăng ký tài khoản thành công";
                    var body = $"<p>Chào {System.Net.WebUtility.HtmlEncode(model.Fullname ?? model.Username)},</p>" +
                               "<p>Bạn đã đăng ký tài khoản thành công tại website của chúng tôi.</p>" +
                               "<p>Trân trọng,</p><p>Ban quản trị</p>";
                    await _emailService.SendEmailAsync(model.Email, subject, body);
                }
                catch
                {
                    // Don't fail registration if email sending fails
                }
            }

            TempData["Success"] = "Đăng ký thành công. Vui lòng kiểm tra email (nếu có) và đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        // Login (GET)
        public IActionResult Login() => View();

        // Login (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(NT.SHARED.DTOs.LoginDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var users = await _repository.FindAsync(u => u.Username == dto.Username);
            var user = System.Linq.Enumerable.FirstOrDefault(users);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(dto);
            }

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verify == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                // NOTE: No authentication cookie implemented here. If you want persistent login,
                // integrate cookie authentication or ASP.NET Identity.
                TempData["Success"] = "Đăng nhập thành công.";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return NotFound();
            var roles = await _roleRepository.GetAllAsync();
            ViewBag.Roles = roles;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, User model)
        {
            if (id == Guid.Empty || model == null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(await _roleRepository.GetAllAsync(), "Id", "Name");
                return View(model);
            }
            await _repository.UpdateAsync(model);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
