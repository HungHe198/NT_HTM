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
    }

    public class TogglePermissionRequest
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
