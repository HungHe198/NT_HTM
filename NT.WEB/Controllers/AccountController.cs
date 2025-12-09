using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using NT.SHARED.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

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
            if (ModelState.IsValid) return View(dto);

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
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim("Fullname", user.Fullname ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = dto.RememberMe,
                    AllowRefresh = true
                });
                TempData["Success"] = "Đăng nhập thành công.";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(dto);
        }

        // Customer Registration (GET)
        public async Task<IActionResult> RegisterCustomer()
        {
            // Ensure Customer role exists for info in view if needed
            var customerRole = await _roleRepository.FindAsync(r => r.Name == "Customer");
            ViewBag.CustomerRoleId = System.Linq.Enumerable.FirstOrDefault(customerRole)?.Id;
            return View();
        }

        // Customer Registration (POST) - creates user with Customer role and sends 6-digit email confirmation code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCustomer(User model)
        {
            // Manual field-level validations to provide specific error messages
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập là bắt buộc");
            }
            else if (model.Username.Length > 100)
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập không được vượt quá 100 ký tự");
            }

            // Using PasswordHash as input field for plain password per current logic
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
            {
                ModelState.AddModelError(nameof(model.PasswordHash), "Mật khẩu là bắt buộc");
            }
            else if (model.PasswordHash.Length < 6)
            {
                ModelState.AddModelError(nameof(model.PasswordHash), "Mật khẩu phải có ít nhất 6 ký tự");
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var emailAttr = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
                if (!emailAttr.IsValid(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Email), "Email không hợp lệ");
                }
            }
            else
            {
                ModelState.AddModelError(nameof(model.Email), "Email là bắt buộc");
            }

            if (string.IsNullOrWhiteSpace(model.Fullname))
            {
                ModelState.AddModelError(nameof(model.Fullname), "Họ và tên là bắt buộc");
            }
            else if (model.Fullname.Length > 200)
            {
                ModelState.AddModelError(nameof(model.Fullname), "Họ và tên không được vượt quá 200 ký tự");
            }

            if (!ModelState.IsValid)
            {
                model.RoleId = _roleRepository.GetAllAsync().Result.FirstOrDefault(r => r.Name == "Customer")?.Id ?? Guid.Empty; // Ensure role is reset for re-display
                return View(model);
            }

            var existing = await _repository.FindAsync(u => u.Username == model.Username);
            if (existing is not null && System.Linq.Enumerable.Any(existing))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại");
                return View(model);
            }

            // Hash password (model.PasswordHash expected to hold plain password from form)
            var plain = model.PasswordHash ?? string.Empty;
            model.PasswordHash = _passwordHasher.HashPassword(model, plain);

            // Force role to Customer
            if (model.RoleId == Guid.Empty)
            {
                var customers = await _roleRepository.FindAsync(r => r.Name == "Customer");
                var role = System.Linq.Enumerable.FirstOrDefault(customers);
                if (role != null)
                {
                    model.RoleId = role.Id;
                }
            }

            await _repository.AddAsync(model);
            await _repository.SaveChangesAsync();

            // Generate 6-digit confirmation code and send email
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                try
                {
                    var random = new Random();
                    var code = random.Next(100000, 999999).ToString();
                    TempData[$"EmailCode:{model.Email}"] = code; // temporary storage for subsequent verification step
                    var subject = "Mã xác nhận tài khoản";
                    var body = $"<p>Chào {System.Net.WebUtility.HtmlEncode(model.Fullname ?? model.Username)},</p>" +
                               $"<p>Mã xác nhận của bạn là: <strong>{code}</strong></p>" +
                               "<p>Vui lòng nhập mã này để xác nhận email.</p>" +
                               "<p>Trân trọng,</p><p>Ban quản trị</p>";
                    await _emailService.SendEmailAsync(model.Email, subject, body);
                    TempData["Success"] = "Đăng ký thành công. Mã xác nhận đã được gửi tới email của bạn.";
                }
                catch
                {
                    TempData["Warning"] = "Đăng ký thành công nhưng gửi email mã xác nhận thất bại.";
                }
            }

            return RedirectToAction(nameof(Login));
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
