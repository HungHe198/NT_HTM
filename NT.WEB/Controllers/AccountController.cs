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
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly Microsoft.AspNetCore.Identity.IPasswordHasher<User> _passwordHasher;
        private readonly Services.IEmailService _emailService;

        public AccountController(IGenericRepository<User> repository, Microsoft.AspNetCore.Identity.IPasswordHasher<User> passwordHasher, Services.IEmailService emailService, IGenericRepository<NT.SHARED.Models.Role> roleRepository, IGenericRepository<Customer> customerRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
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
        public IActionResult Login(string? msg = null)
        {
            if (string.Equals(msg, "login_required", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Vui lòng đăng nhập để tiếp tục";
            }
            return View();
        }

        // Login (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(NT.SHARED.DTOs.LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu đăng nhập không hợp lệ";
                return View(dto);
            }

            var users = await _repository.FindAsync(u => u.Username == dto.Username);
            var user = System.Linq.Enumerable.FirstOrDefault(users);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View(dto);
            }

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verify == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                // Resolve role name for correct role claim
                string roleName = string.Empty;
                try
                {
                    var role = await _roleRepository.GetByIdAsync(user.RoleId);
                    roleName = role?.Name ?? string.Empty;
                }
                catch { /* ignore */ }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim("Fullname", user.Fullname ?? string.Empty),
                    new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(roleName) ? "User" : roleName)
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
            TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View(dto);
        }

        // Customer Registration (GET)
        public async Task<IActionResult> RegisterCustomer(string? msg = null)
        {
            if (string.Equals(msg, "customer_required", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Chỉ khách hàng mới được truy cập tính năng này. Vui lòng đăng nhập hoặc đăng ký tài khoản khách hàng.";
            }
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

            // Phone number is required for customer registration
            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Số điện thoại là bắt buộc");
            }
            else if (model.PhoneNumber.Length > 20)
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Số điện thoại không được vượt quá 20 ký tự");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu đăng ký không hợp lệ. Vui lòng kiểm tra lại.";
                model.RoleId = _roleRepository.GetAllAsync().Result.FirstOrDefault(r => r.Name == "Customer")?.Id ?? Guid.Empty; // Ensure role is reset for re-display
                return View(model);
            }

            var existing = await _repository.FindAsync(u => u.Username == model.Username);
            if (existing is not null && System.Linq.Enumerable.Any(existing))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại");
                TempData["Error"] = "Tên đăng nhập đã tồn tại";
                return View(model);
            }

            // Default status for new customer accounts
            model.Status = "1";

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

            // Automatically create Customer record to ensure user can add to cart immediately
            var customer = Customer.Create(model.Id);
            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

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
            else
            {
                TempData["Success"] = "Đăng ký thành công. Vui lòng đăng nhập.";
            }

            // Redirect to login page directly - Customer record already created
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

        // Profile (GET) - Xem và chỉnh sửa thông tin cá nhân
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Lấy UserId từ claims
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem thông tin cá nhân";
                return RedirectToAction(nameof(Login));
            }

            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin tài khoản";
                return RedirectToAction(nameof(Login));
            }

            // Tạo DTO với dữ liệu hiện tại
            var dto = new NT.SHARED.DTOs.UpdateProfileDto
            {
                Fullname = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            // Nếu là Customer, lấy thêm thông tin Customer
            var customers = await _customerRepository.FindAsync(c => c.UserId == userId);
            var customer = System.Linq.Enumerable.FirstOrDefault(customers);
            if (customer != null)
            {
                dto.Address = customer.Address;
                dto.DoB = customer.DoB;
                dto.Gender = customer.Gender;
            }

            // Truyền Username để hiển thị (không cho sửa)
            ViewBag.Username = user.Username;
            ViewBag.IsCustomer = customer != null;

            return View(dto);
        }

        // Profile (POST) - Cập nhật thông tin cá nhân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(NT.SHARED.DTOs.UpdateProfileDto dto)
        {
            // Lấy UserId từ claims
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Vui lòng đăng nhập để cập nhật thông tin";
                return RedirectToAction(nameof(Login));
            }

            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin tài khoản";
                return RedirectToAction(nameof(Login));
            }

            // Kiểm tra Customer
            var customers = await _customerRepository.FindAsync(c => c.UserId == userId);
            var customer = System.Linq.Enumerable.FirstOrDefault(customers);

            ViewBag.Username = user.Username;
            ViewBag.IsCustomer = customer != null;

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return View(dto);
            }

            // Cập nhật thông tin User (chỉ các trường được phép)
            user.Fullname = dto.Fullname;
            user.PhoneNumber = dto.PhoneNumber;
            user.Email = dto.Email;
            // Không thay đổi: Id, RoleId, Username, PasswordHash, Status

            await _repository.UpdateAsync(user);
            await _repository.SaveChangesAsync();

            // Nếu là Customer, cập nhật thêm thông tin Customer
            if (customer != null)
            {
                customer.Address = dto.Address;
                customer.DoB = dto.DoB;
                customer.Gender = dto.Gender;
                // Không thay đổi: Id, UserId

                await _customerRepository.UpdateAsync(customer);
                await _customerRepository.SaveChangesAsync();
            }

            TempData["Success"] = "Cập nhật thông tin cá nhân thành công!";
            return RedirectToAction(nameof(Profile));
        }

        // Logout (GET) to support link-based sign out from client layout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Sign out authentication cookie
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            // Clear server-side session
            HttpContext.Session.Clear();
            TempData["Success"] = "Đã đăng xuất";
            return RedirectToAction(nameof(Login));
        }

        // Logout (POST) for form-based sign out if needed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPost()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            TempData["Success"] = "Đã đăng xuất";
            return RedirectToAction(nameof(Login));
        }
    }
}
