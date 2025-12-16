using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.WEB.Authorization
{
    /// <summary>
    /// Service quản lý quyền hạn cho Role.
    /// Cung cấp các chức năng: kiểm tra quyền, gán/gỡ quyền, đồng bộ Permission từ Endpoint.
    /// </summary>
    public class RolePermissionService
    {
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<Permission> _permissionRepo;
        private readonly IGenericRepository<RolePermission> _rolePermissionRepo;
        private readonly EndpointScannerService _endpointScanner;

        public RolePermissionService(
            IGenericRepository<Role> roleRepo,
            IGenericRepository<Permission> permissionRepo,
            IGenericRepository<RolePermission> rolePermissionRepo,
            EndpointScannerService endpointScanner)
        {
            _roleRepo = roleRepo;
            _permissionRepo = permissionRepo;
            _rolePermissionRepo = rolePermissionRepo;
            _endpointScanner = endpointScanner;
        }

        /// <summary>
        /// Kiểm tra Role có Permission hay không
        /// </summary>
        public async Task<bool> HasPermissionAsync(Guid roleId, string resource, string action)
        {
            if (roleId == Guid.Empty) return false;

            // Lấy Permission theo resource và action
            var permissions = await _permissionRepo.FindAsync(p =>
                p.Resource == resource && p.Action == action);
            var permission = permissions.FirstOrDefault();

            if (permission == null) return false;

            // Kiểm tra RolePermission
            var rolePermissions = await _rolePermissionRepo.FindAsync(rp =>
                rp.RoleId == roleId && rp.PermissionId == permission.Id);

            return rolePermissions.Any();
        }

        /// <summary>
        /// Kiểm tra Role có Permission theo Code hay không
        /// </summary>
        public async Task<bool> HasPermissionByCodeAsync(Guid roleId, string permissionCode)
        {
            if (roleId == Guid.Empty || string.IsNullOrWhiteSpace(permissionCode)) return false;

            var permissions = await _permissionRepo.FindAsync(p => p.Code == permissionCode);
            var permission = permissions.FirstOrDefault();

            if (permission == null) return false;

            var rolePermissions = await _rolePermissionRepo.FindAsync(rp =>
                rp.RoleId == roleId && rp.PermissionId == permission.Id);

            return rolePermissions.Any();
        }

        /// <summary>
        /// Lấy tất cả Permission của một Role
        /// </summary>
        public async Task<IEnumerable<Permission>> GetPermissionsForRoleAsync(Guid roleId)
        {
            if (roleId == Guid.Empty) return Enumerable.Empty<Permission>();

            var rolePermissions = await _rolePermissionRepo.FindAsync(
                rp => rp.RoleId == roleId,
                rp => rp.Permission!);

            return rolePermissions
                .Where(rp => rp.Permission != null)
                .Select(rp => rp.Permission!)
                .ToList();
        }

        /// <summary>
        /// Gán Permission cho Role
        /// </summary>
        public async Task AssignPermissionAsync(Guid roleId, Guid permissionId)
        {
            if (roleId == Guid.Empty || permissionId == Guid.Empty)
                throw new ArgumentException("RoleId và PermissionId không được rỗng");

            // Kiểm tra đã tồn tại chưa
            var existing = await _rolePermissionRepo.FindAsync(rp =>
                rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (existing.Any()) return; // Đã có rồi

            var rolePermission = RolePermission.Create(roleId, permissionId);
            await _rolePermissionRepo.AddAsync(rolePermission);
        }

        /// <summary>
        /// Gỡ Permission khỏi Role
        /// </summary>
        public async Task RemovePermissionAsync(Guid roleId, Guid permissionId)
        {
            if (roleId == Guid.Empty || permissionId == Guid.Empty)
                throw new ArgumentException("RoleId và PermissionId không được rỗng");

            await _rolePermissionRepo.DeleteWhereAsync(rp =>
                rp.RoleId == roleId && rp.PermissionId == permissionId);
        }

        /// <summary>
        /// Cập nhật tất cả Permission cho Role (thay thế hoàn toàn)
        /// </summary>
        public async Task UpdateRolePermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds)
        {
            if (roleId == Guid.Empty)
                throw new ArgumentException("RoleId không được rỗng");

            // Xóa tất cả permission cũ của role
            await _rolePermissionRepo.DeleteWhereAsync(rp => rp.RoleId == roleId);

            // Thêm các permission mới
            foreach (var permissionId in permissionIds.Distinct())
            {
                if (permissionId != Guid.Empty)
                {
                    var rolePermission = RolePermission.Create(roleId, permissionId);
                    await _rolePermissionRepo.AddAsync(rolePermission);
                }
            }
        }

        /// <summary>
        /// Đồng bộ Permission từ các Endpoint đã quét vào database.
        /// Tạo mới Permission nếu chưa tồn tại.
        /// </summary>
        public async Task<int> SyncPermissionsFromEndpointsAsync()
        {
            var endpoints = _endpointScanner.ScanAllEndpoints();
            var existingPermissions = await _permissionRepo.GetAllAsync();
            var existingCodes = existingPermissions.Select(p => p.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var newCount = 0;

            foreach (var endpoint in endpoints)
            {
                if (!existingCodes.Contains(endpoint.PermissionCode))
                {
                    var permission = Permission.Create(
                        code: endpoint.PermissionCode,
                        description: endpoint.Description,
                        resource: endpoint.Controller,
                        action: endpoint.Action,
                        method: endpoint.HttpMethod
                    );

                    await _permissionRepo.AddAsync(permission);
                    newCount++;
                }
            }

            return newCount;
        }

        /// <summary>
        /// Lấy tất cả Permission được nhóm theo Resource
        /// </summary>
        public async Task<Dictionary<string, List<Permission>>> GetPermissionsGroupedByResourceAsync()
        {
            var permissions = await _permissionRepo.GetAllAsync();

            return permissions
                .Where(p => !string.IsNullOrWhiteSpace(p.Resource))
                .GroupBy(p => p.Resource!)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(p => p.Action).ToList()
                );
        }

        /// <summary>
        /// Lấy Permission IDs của một Role
        /// </summary>
        public async Task<HashSet<Guid>> GetPermissionIdsForRoleAsync(Guid roleId)
        {
            if (roleId == Guid.Empty) return new HashSet<Guid>();

            var rolePermissions = await _rolePermissionRepo.FindAsync(rp => rp.RoleId == roleId);
            return rolePermissions.Select(rp => rp.PermissionId).ToHashSet();
        }
    }
}
