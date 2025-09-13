using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Username { get; private set; } = null!;
        [Required]
        public string PasswordHash { get; private set; } = null!;
        [Required]
        public Guid ActorTypeId { get; private set; }

        // Private constructor for EF
        private User() { }

        // Public static factory
        public static User Create(string username, string passwordHash, Guid actorTypeId)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Không được để trống tài khoản", nameof(username));
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Mật khẩu không chính xác", nameof(passwordHash));
            if (actorTypeId == Guid.Empty) throw new ArgumentException("Tài khoản chưa được cấp quyền sử dụng chức năng này", nameof(actorTypeId));
            return new User
            {
                Username = username,
                PasswordHash = passwordHash,
                ActorTypeId = actorTypeId
            };
        }

        // Navigation
        public ActorType ActorType { get; private set; } = null!;
        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
        public ICollection<Order> Orders { get; private set; } = new List<Order>();
    }
}