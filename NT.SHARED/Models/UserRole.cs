using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class UserRole
    {
        [Required]
        public Guid UserId { get; private set; }
        [Required]
        public Guid RoleId { get; private set; }

        // Private constructor for EF
        private UserRole() { }

        // Public static factory
        public static UserRole Create(Guid userId, Guid roleId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("Invalid user ID", nameof(userId));
            if (roleId == Guid.Empty) throw new ArgumentException("Invalid role ID", nameof(roleId));
            return new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };
        }

        // Navigation
        public User User { get; private set; } = null!;
        public Role Role { get; private set; } = null!;
    }
}