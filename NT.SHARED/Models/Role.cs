using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Role
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Name { get; private set; } = null!;

        private Role() { }

        public static Role Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên role không hợp lệ");
            return new Role { Name = name };
        }

        public ICollection<User> Users { get; private set; } = new List<User>();
        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();
    }

}