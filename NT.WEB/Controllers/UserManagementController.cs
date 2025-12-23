using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using NT.WEB.Authorization;
using System.Security.Claims;

namespace NT.WEB.Controllers
{
    /// <summary>
    /// Controller quản lý tất cả tài khoản người dùng trong hệ thống.
    /// Chỉ Admin hoặc Employee có quyền mới được truy cập.
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class UserManagementController : Controller
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<Admin> _adminRepo;
        private readonly Microsoft.AspNetCore.Identity.IPasswordHasher<User> _passwordHasher;

        public UserManagementController(
            IGenericRepository<User> userRepo,
            IGenericRepository<Role> roleRepo,
            IGenericRepository<Customer> customerRepo,
            IGenericRepository<Employee> employeeRepo,
            IGenericRepository<Admin> adminRepo,
            Microsoft.AspNetCore.Identity.IPasswordHasher<User> passwordHasher)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _customerRepo = customerRepo;
            _employeeRepo = employeeRepo;
            _adminRepo = adminRepo;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Danh sách tất cả tài khoản
        /// </summary>
        [RequirePermission("UserManagement", "Index")]
        public async Task<IActionResult> Index(string? search, string? roleFilter, string? statusFilter)
        {
            var users = await _userRepo.GetAllAsync();
            var roles = await _roleRepo.GetAllAsync();

            // Load role cho mỗi user
            foreach (var u in users ?? Array.Empty<User>())
            {
                if (u.Role == null && u.RoleId != Guid.Empty)
                {
                    u.Role = roles.FirstOrDefault(r => r.Id == u.RoleId);
                }
            }

            // Filter
            var query = users?.AsQueryable() ?? Enumerable.Empty<User>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(u =>
                    (u.Username != null && u.Username.ToLower().Contains(search)) ||
                    (u.Fullname != null && u.Fullname.ToLower().Contains(search)) ||
                    (u.Email != null && u.Email.ToLower().Contains(search)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(roleFilter) && Guid.TryParse(roleFilter, out var roleId))
            {
                query = query.Where(u => u.RoleId == roleId);
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(u => u.Status == statusFilter);
            }

            ViewBag.Roles = roles;
            ViewBag.Search = search;
            ViewBag.RoleFilter = roleFilter;
            ViewBag.StatusFilter = statusFilter;

            // Thống kê
            ViewBag.TotalUsers = users?.Count() ?? 0;
            ViewBag.ActiveUsers = users?.Count(u => u.Status == "1" || u.Status == "Active") ?? 0;
            ViewBag.AdminCount = users?.Count(u => u.Role?.Name == "Admin") ?? 0;
            ViewBag.EmployeeCount = users?.Count(u => u.Role?.Name == "Employee") ?? 0;
            ViewBag.CustomerCount = users?.Count(u => u.Role?.Name == "Customer") ?? 0;

            return View(query.OrderBy(u => u.Username).ToList());
        }

        /// <summary>
        /// Chi tiết tài khoản
        /// </summary>
        [RequirePermission("UserManagement", "Details")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            // Load role
            if (user.Role == null && user.RoleId != Guid.Empty)
            {
                user.Role = await _roleRepo.GetByIdAsync(user.RoleId);
            }

            // Load thông tin liên quan (Customer/Employee/Admin)
            var customers = await _customerRepo.FindAsync(c => c.UserId == id);
            var employees = await _employeeRepo.FindAsync(e => e.UserId == id);
            var admins = await _adminRepo.FindAsync(a => a.UserId == id);

            ViewBag.Customer = customers.FirstOrDefault();
            ViewBag.Employee = employees.FirstOrDefault();
            ViewBag.Admin = admins.FirstOrDefault();

            return View(user);
        }

        /// <summary>
        /// Tạo tài khoản mới (GET)
        /// </summary>
        [RequirePermission("UserManagement", "Create")]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleRepo.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View();
        }

        /// <summary>
        /// Tạo tài khoản mới (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("UserManagement", "Create")]
        public async Task<IActionResult> Create(User model, string? plainPassword)
        {
            var roles = await _roleRepo.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name", model.RoleId);

            // Xóa validation error cho PasswordHash vì chúng ta xử lý riêng qua plainPassword
            ModelState.Remove(nameof(model.PasswordHash));

            // Validation thủ công
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập là bắt buộc");
            }
            else
            {
                // đảm bảo username chỉ chứa ký tự hợp lệ
                var unameRe = new System.Text.RegularExpressions.Regex("^[A-Za-z0-9._-]+$");
                if (!unameRe.IsMatch(model.Username))
                {
                    ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập phải viết liền, không dấu, không chứa khoảng trắng; chỉ dùng chữ/số/./_/-.");
                }
            }

            if (string.IsNullOrWhiteSpace(plainPassword))
            {
                ModelState.AddModelError("plainPassword", "Mật khẩu là bắt buộc");
            }
            else if (plainPassword.Length < 6)
            {
                ModelState.AddModelError("plainPassword", "Mật khẩu phải có ít nhất 6 ký tự");
            }

            if (string.IsNullOrWhiteSpace(model.Fullname))
            {
                ModelState.AddModelError(nameof(model.Fullname), "Họ và tên là bắt buộc");
            }

            if (model.RoleId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.RoleId), "Vui lòng chọn vai trò");
            }

            // Check username exists
            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                var existing = await _userRepo.FindAsync(u => u.Username == model.Username);
                if (existing.Any())
                {
                    ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại");
                }
            }

            // Check phone number exists (nếu có nhập số điện thoại)
            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                var phoneExists = await _userRepo.FindAsync(u => u.PhoneNumber == model.PhoneNumber.Trim());
                if (phoneExists.Any())
                {
                    ModelState.AddModelError(nameof(model.PhoneNumber), "Số điện thoại đã được sử dụng bởi tài khoản khác");
                }
            }

            // Check email exists (nếu có nhập email)
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var emailExists = await _userRepo.FindAsync(u => u.Email == model.Email.Trim());
                if (emailExists.Any())
                {
                    ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng bởi tài khoản khác");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Hash password
            model.PasswordHash = _passwordHasher.HashPassword(model, plainPassword!);
            model.Status = "1"; // Active

            await _userRepo.AddAsync(model);
            await _userRepo.SaveChangesAsync();

            // Tạo record liên quan dựa trên role
            var role = roles.FirstOrDefault(r => r.Id == model.RoleId);
            if (role != null)
            {
                switch (role.Name)
                {
                    case "Customer":
                        var customer = Customer.Create(model.Id);
                        await _customerRepo.AddAsync(customer);
                        await _customerRepo.SaveChangesAsync();
                        break;
                    case "Employee":
                        var employee = Employee.Create(model.Id);
                        await _employeeRepo.AddAsync(employee);
                        await _employeeRepo.SaveChangesAsync();
                        break;
                    case "Admin":
                        var admin = Admin.Create(model.Id);
                        await _adminRepo.AddAsync(admin);
                        await _adminRepo.SaveChangesAsync();
                        break;
                }
            }

            TempData["Success"] = $"Đã tạo tài khoản '{model.Username}' thành công";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Sửa tài khoản (GET)
        /// </summary>
        [RequirePermission("UserManagement", "Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _roleRepo.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name", user.RoleId);

            return View(user);
        }

        /// <summary>
        /// Sửa tài khoản (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("UserManagement", "Edit")]
        public async Task<IActionResult> Edit(Guid id, User model, string? newPassword)
        {
            if (id == Guid.Empty || model.Id != id) return BadRequest();

            var roles = await _roleRepo.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name", model.RoleId);

            var existingUser = await _userRepo.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            // Xóa validation error cho PasswordHash vì chúng ta xử lý riêng
            ModelState.Remove(nameof(model.PasswordHash));

            // Validation thủ công
            if (string.IsNullOrWhiteSpace(model.Fullname))
            {
                ModelState.AddModelError(nameof(model.Fullname), "Họ và tên là bắt buộc");
            }

            // Validate username on edit as well (cannot contain spaces or diacritics)
            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                var unameRe = new System.Text.RegularExpressions.Regex("^[A-Za-z0-9._-]+$");
                if (!unameRe.IsMatch(model.Username))
                {
                    ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập phải viết liền, không dấu, không chứa khoảng trắng; chỉ dùng chữ/số/./_/-.");
                }
            }

            if (model.RoleId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.RoleId), "Vui lòng chọn vai trò");
            }

            // Check phone number exists (trừ user hiện tại)
            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                var phoneExists = await _userRepo.FindAsync(u => u.PhoneNumber == model.PhoneNumber.Trim() && u.Id != id);
                if (phoneExists.Any())
                {
                    ModelState.AddModelError(nameof(model.PhoneNumber), "Số điện thoại đã được sử dụng bởi tài khoản khác");
                }
            }

            // Check email exists (trừ user hiện tại)
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var emailExists = await _userRepo.FindAsync(u => u.Email == model.Email.Trim() && u.Id != id);
                if (emailExists.Any())
                {
                    ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng bởi tài khoản khác");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Update fields
            existingUser.Fullname = model.Fullname;
            existingUser.Email = model.Email;
            existingUser.PhoneNumber = model.PhoneNumber;
            existingUser.Status = model.Status;

            // Check if role changed
            var oldRoleId = existingUser.RoleId;
            existingUser.RoleId = model.RoleId;

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword.Length < 6)
                {
                    ModelState.AddModelError("newPassword", "Mật khẩu phải có ít nhất 6 ký tự");
                    return View(model);
                }
                existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, newPassword);
            }

            await _userRepo.UpdateAsync(existingUser);
            await _userRepo.SaveChangesAsync();

            TempData["Success"] = $"Đã cập nhật tài khoản '{existingUser.Username}' thành công";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Xóa tài khoản (GET - confirm)
        /// </summary>
        [RequirePermission("UserManagement", "Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            // Prevent self-delete
            var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(currentUserId, out var uid) && uid == id)
            {
                TempData["Error"] = "Bạn không thể xóa chính tài khoản của mình";
                return RedirectToAction(nameof(Index));
            }

            // Load role
            if (user.Role == null && user.RoleId != Guid.Empty)
            {
                user.Role = await _roleRepo.GetByIdAsync(user.RoleId);
            }

            return View(user);
        }

        /// <summary>
        /// Xóa tài khoản (POST)
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RequirePermission("UserManagement", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            // Prevent self-delete
            var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(currentUserId, out var uid) && uid == id)
            {
                TempData["Error"] = "Bạn không thể xóa chính tài khoản của mình";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            // Xóa các record liên quan trước
            var customers = await _customerRepo.FindAsync(c => c.UserId == id);
            foreach (var c in customers)
            {
                await _customerRepo.DeleteAsync(c.Id);
            }

            var employees = await _employeeRepo.FindAsync(e => e.UserId == id);
            foreach (var e in employees)
            {
                await _employeeRepo.DeleteAsync(e.Id);
            }

            var admins = await _adminRepo.FindAsync(a => a.UserId == id);
            foreach (var a in admins)
            {
                await _adminRepo.DeleteAsync(a.Id);
            }

            await _userRepo.DeleteAsync(id);
            await _userRepo.SaveChangesAsync();

            TempData["Success"] = $"Đã xóa tài khoản '{user.Username}' thành công";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Thay đổi trạng thái tài khoản (Active/Inactive)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("UserManagement", "ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            // Toggle status
            user.Status = (user.Status == "1" || user.Status == "Active") ? "0" : "1";

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();

            var statusText = user.Status == "1" ? "kích hoạt" : "vô hiệu hóa";
            TempData["Success"] = $"Đã {statusText} tài khoản '{user.Username}'";

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Reset mật khẩu tài khoản
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("UserManagement", "ResetPassword")]
        public async Task<IActionResult> ResetPassword(Guid id, string newPassword)
        {
            if (id == Guid.Empty) return BadRequest();

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                TempData["Error"] = "Mật khẩu phải có ít nhất 6 ký tự";
                return RedirectToAction(nameof(Edit), new { id });
            }

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();

            TempData["Success"] = $"Đã đặt lại mật khẩu cho tài khoản '{user.Username}'";
            return RedirectToAction(nameof(Index));
        }
    }
}
