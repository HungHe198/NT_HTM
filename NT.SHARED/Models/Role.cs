using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Role
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(150), Display(Name = "Vai trò")]
        public string Name { get; private set; } = null!;

        private Role() { }
        public static Role Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui lòng nhập tên vai trò");
            return new Role { Name = name.Trim() };
        }

        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();
        public ICollection<User> Users { get; private set; } = new List<User>();
    }
}
