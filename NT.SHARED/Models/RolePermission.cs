using System;

namespace NT.SHARED.Models
{
    public class RolePermission
    {
        public Guid RoleId { get; private set; }
        public Guid PermissionId { get; private set; }

        private RolePermission() { }
        public static RolePermission Create(Guid roleId, Guid permissionId)
        {
            if (roleId == Guid.Empty || permissionId == Guid.Empty) throw new ArgumentException("Invalid ids");
            return new RolePermission { RoleId = roleId, PermissionId = permissionId };
        }

        public Role Role { get; private set; } = null!;
        public Permission Permission { get; private set; } = null!;
    }
}
