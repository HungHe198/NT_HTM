using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class RolePermission
    {
        [Display(Name = "Vai trò")]
        public Guid RoleId { get; private set; }
        [Display(Name = "Quyền")]
        public Guid PermissionId { get; private set; }

        private RolePermission() { }
        public static RolePermission Create(Guid roleId, Guid permissionId)
        {
            if (roleId == Guid.Empty || permissionId == Guid.Empty) throw new ArgumentException("Id vai trị hoặc quyền không hợp lí");
            return new RolePermission { RoleId = roleId, PermissionId = permissionId };
        }

        public Role? Role { get; private set; }  
        public Permission? Permission { get; private set; }   
    }
}
