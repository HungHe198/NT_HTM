using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class RolePermission
    {
        [Required]
        public Guid RoleId { get; private set; }
        [Required]
        public Guid PermissionId { get; private set; }

        // Private constructor for EF
        private RolePermission() { }

        // Public static factory
        public static RolePermission Create(Guid roleId, Guid permissionId)
        {
            if (roleId == Guid.Empty) throw new ArgumentException("Invalid role ID", nameof(roleId));
            if (permissionId == Guid.Empty) throw new ArgumentException("Invalid permission ID", nameof(permissionId));
            return new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };
        }

        // Navigation
        public Role Role { get; private set; } = null!;
        public Permission Permission { get; private set; } = null!;
    }
}