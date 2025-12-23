using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using NT.WEB.Authorization;
using NT.WEB.ViewModels;

namespace NT.WEB.Controllers
{
    /// <summary>
    /// Controller quản lý phân quyền cho Role.
    /// Cho phép Admin gán/gỡ Permission cho từng Role.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class RolePermissionController : Controller
    {
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<Permission> _permissionRepo;
        private readonly IGenericRepository<RolePermission> _rolePermissionRepo;
        private readonly RolePermissionService _rolePermissionService;
        private readonly EndpointScannerService _endpointScanner;

        public RolePermissionController(
            IGenericRepository<Role> roleRepo,
            IGenericRepository<Permission> permissionRepo,
            IGenericRepository<RolePermission> rolePermissionRepo,
            RolePermissionService rolePermissionService,
            EndpointScannerService endpointScanner)
        {
            _roleRepo = roleRepo;
            _permissionRepo = permissionRepo;
            _rolePermissionRepo = rolePermissionRepo;
            _rolePermissionService = rolePermissionService;
            _endpointScanner = endpointScanner;
        }

        /// <summary>
        /// Danh sách Role và số lượng quyền
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var roles = await _roleRepo.GetAllAsync();
            var rolePermissions = await _rolePermissionRepo.GetAllAsync();

            var viewModel = roles.Select(r => new RoleListViewModel
            {
                Id = r.Id,
                Name = r.Name,
                PermissionCount = rolePermissions.Count(rp => rp.RoleId == r.Id)
            }).OrderBy(r => r.Name).ToList();

            return View(viewModel);
        }

        /// <summary>
        /// Trang phân quyền chi tiết cho một Role
        /// </summary>
        public async Task<IActionResult> Manage(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var role = await _roleRepo.GetByIdAsync(id);
            if (role == null) return NotFound();

            // Lấy tất cả Permission
            var allPermissions = await _permissionRepo.GetAllAsync();

            // Lấy Permission đã gán cho Role
            var assignedIds = await _rolePermissionService.GetPermissionIdsForRoleAsync(id);

            // Nhóm Permission theo Resource
            var permissionsByResource = allPermissions
                .Where(p => !string.IsNullOrWhiteSpace(p.Resource))
                .GroupBy(p => p.Resource!)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(p => new PermissionItemViewModel
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Description = p.Description,
                        Resource = p.Resource,
                        Action = p.Action,
                        Method = p.Method,
                        IsAssigned = assignedIds.Contains(p.Id)
                    }).OrderBy(p => p.Action).ToList()
                );

            var viewModel = new RolePermissionViewModel
            {
                Role = role,
                PermissionsByResource = permissionsByResource,
                AssignedPermissionIds = assignedIds
            };

            return View(viewModel);
        }

        /// <summary>
        /// Cập nhật quyền cho Role
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePermissions(UpdateRolePermissionsRequest request)
        {
            if (request.RoleId == Guid.Empty)
            {
                TempData["Error"] = "Role không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _rolePermissionService.UpdateRolePermissionsAsync(request.RoleId, request.PermissionIds ?? new List<Guid>());
                TempData["Success"] = "Cập nhật quyền thành công";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi cập nhật quyền: {ex.Message}";
            }

            return RedirectToAction(nameof(Manage), new { id = request.RoleId });
        }

        /// <summary>
        /// Toggle một Permission cho Role (AJAX)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> TogglePermission([FromBody] TogglePermissionRequest request)
        {
            if (request.RoleId == Guid.Empty || request.PermissionId == Guid.Empty)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                var hasPermission = await _rolePermissionService.HasPermissionByCodeAsync(request.RoleId, string.Empty);
                
                // Kiểm tra Permission có được gán chưa
                var assignedIds = await _rolePermissionService.GetPermissionIdsForRoleAsync(request.RoleId);
                var isCurrentlyAssigned = assignedIds.Contains(request.PermissionId);

                if (isCurrentlyAssigned)
                {
                    await _rolePermissionService.RemovePermissionAsync(request.RoleId, request.PermissionId);
                    return Json(new { success = true, assigned = false, message = "Đã gỡ quyền" });
                }
                else
                {
                    await _rolePermissionService.AssignPermissionAsync(request.RoleId, request.PermissionId);
                    return Json(new { success = true, assigned = true, message = "Đã gán quyền" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Đồng bộ Permission từ các Endpoint
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SyncPermissions()
        {
            try
            {
                var newCount = await _rolePermissionService.SyncPermissionsFromEndpointsAsync();
                TempData["Success"] = $"Đã đồng bộ thành công. Thêm mới {newCount} quyền.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi đồng bộ: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Xem danh sách Endpoint được quét
        /// </summary>
        public IActionResult Endpoints()
        {
            var endpoints = _endpointScanner.ScanAllEndpoints();
            return View(endpoints);
        }

        /// <summary>
        /// Gán tất cả quyền cho Role
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignAllPermissions(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                TempData["Error"] = "Role không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var allPermissions = await _permissionRepo.GetAllAsync();
                var permissionIds = allPermissions.Select(p => p.Id).ToList();
                await _rolePermissionService.UpdateRolePermissionsAsync(roleId, permissionIds);
                TempData["Success"] = "Đã gán tất cả quyền cho role";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction(nameof(Manage), new { id = roleId });
        }

        /// <summary>
        /// Gỡ tất cả quyền khỏi Role
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAllPermissions(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                TempData["Error"] = "Role không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _rolePermissionService.UpdateRolePermissionsAsync(roleId, new List<Guid>());
                TempData["Success"] = "Đã gỡ tất cả quyền khỏi role";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction(nameof(Manage), new { id = roleId });
        }

        /// <summary>
        /// Danh sách các Resource mặc định cho Customer
        /// Customer được phép: Home, Product (xem), Cart, Checkout, Orders (My), Account (Profile)
        /// </summary>
        private static readonly HashSet<string> CustomerDefaultResources = new(StringComparer.OrdinalIgnoreCase)
        {
            "Home", "Product", "Cart", "CartDetail", "Checkout", "Orders", "Account"
        };

        /// <summary>
        /// Danh sách các Action cụ thể cho Customer (chỉ cho phép những action này)
        /// </summary>
        private static readonly Dictionary<string, HashSet<string>> CustomerAllowedActions = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Home", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Index", "GetAllProducts", "Privacy", "Contact", "Error", "AccessDenied" } },
            { "Product", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "ProductDetailIndex", "Suggest", "Details" } },
            { "Cart", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Index", "Add", "UpdateQty", "Remove", "ApplyVoucher", "RemoveVoucher", "Details" } },
            { "CartDetail", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Index", "UpdateQuantity", "Remove" } },
            { "Checkout", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Start", "Submit", "ApplyVoucher", "RemoveVoucher", "ApplyVoucherAjax", "RemoveVoucherAjax" } },
            { "Orders", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "My", "Review", "Details", "Cancel" } },
            { "Account", new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Profile", "Login", "Logout", "RegisterCustomer", "Create" } }
        };

        /// <summary>
        /// Danh sách các Resource mặc định cho Employee
        /// Employee được phép: Tất cả của Customer + Sales, Orders (quản lý), Product (quản lý), Customer (quản lý), UserManagement
        /// </summary>
        private static readonly HashSet<string> EmployeeDefaultResources = new(StringComparer.OrdinalIgnoreCase)
        {
            "Home", "Product", "Cart", "CartDetail", "Checkout", "Orders", "Account",
            "Sales", "Customer", "ProductDetail", "Brand", "Category", "Color", "Length",
            "Hardness", "Elasticity", "SurfaceFinish", "OriginCountry", "ProductImage", "Voucher", "PaymentMethod",
            "UserManagement", "Employee"
        };

        /// <summary>
        /// Gán quyền mặc định cho Customer
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignDefaultCustomerPermissions(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                TempData["Error"] = "Role không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var allPermissions = await _permissionRepo.GetAllAsync();
                var customerPermissionIds = allPermissions
                    .Where(p => !string.IsNullOrWhiteSpace(p.Resource) && !string.IsNullOrWhiteSpace(p.Action))
                    .Where(p =>
                    {
                        // Kiểm tra Resource có trong danh sách cho phép
                        if (!CustomerDefaultResources.Contains(p.Resource!))
                            return false;

                        // Nếu có danh sách action cụ thể, kiểm tra action
                        if (CustomerAllowedActions.TryGetValue(p.Resource!, out var allowedActions))
                        {
                            return allowedActions.Contains(p.Action!);
                        }

                        // Nếu không có danh sách action cụ thể, cho phép tất cả action của resource đó
                        return true;
                    })
                    .Select(p => p.Id)
                    .ToList();

                await _rolePermissionService.UpdateRolePermissionsAsync(roleId, customerPermissionIds);
                await _rolePermissionRepo.SaveChangesAsync();

                TempData["Success"] = $"Đã gán {customerPermissionIds.Count} quyền mặc định cho Customer";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction(nameof(Manage), new { id = roleId });
        }

        /// <summary>
        /// Gán quyền mặc định cho Employee
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignDefaultEmployeePermissions(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                TempData["Error"] = "Role không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var allPermissions = await _permissionRepo.GetAllAsync();
                
                // Employee được phép tất cả permissions của các resource trong danh sách
                // NGOẠI TRỪ: Admin, RolePermission, Employee (quản lý nhân viên khác)
                var excludedResources = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "Admin", "RolePermission"
                };

                var employeePermissionIds = allPermissions
                    .Where(p => !string.IsNullOrWhiteSpace(p.Resource))
                    .Where(p => EmployeeDefaultResources.Contains(p.Resource!) || 
                               (!excludedResources.Contains(p.Resource!)))
                    .Where(p => !excludedResources.Contains(p.Resource!))
                    .Select(p => p.Id)
                    .ToList();

                await _rolePermissionService.UpdateRolePermissionsAsync(roleId, employeePermissionIds);
                await _rolePermissionRepo.SaveChangesAsync();

                TempData["Success"] = $"Đã gán {employeePermissionIds.Count} quyền mặc định cho Employee";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction(nameof(Manage), new { id = roleId });
        }

        /// <summary>
        /// Xem danh sách quyền hiện tại của một Role (API)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRolePermissions(Guid roleId)
        {
            if (roleId == Guid.Empty)
                return Json(new { success = false, message = "Role không hợp lệ" });

            var permissions = await _rolePermissionService.GetPermissionsForRoleAsync(roleId);
            var result = permissions.Select(p => new
            {
                p.Id,
                p.Code,
                p.Resource,
                p.Action,
                p.Description
            }).ToList();

            return Json(new { success = true, count = result.Count, permissions = result });
        }
    }

    public class TogglePermissionRequest
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
