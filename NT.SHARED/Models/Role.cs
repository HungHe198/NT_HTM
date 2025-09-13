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
        [Required, MaxLength(50)]
        public string Code { get; private set; } = null!; // VD: SUPER_ADMIN
        [Required]
        public Guid ActorTypeId { get; private set; }

        // Private constructor for EF
        private Role() { }

        // Public static factory
        public static Role Create(string name, string code, Guid actorTypeId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid name", nameof(name));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Invalid code", nameof(code));
            if (actorTypeId == Guid.Empty) throw new ArgumentException("Invalid actor type ID", nameof(actorTypeId));
            return new Role
            {
                Name = name,
                Code = code,
                ActorTypeId = actorTypeId
            };
        }

        // Navigation
        public ActorType ActorType { get; private set; } = null!;
        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();
        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    }
}