using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid RoleId { get; private set; }
        [Required, MaxLength(100)]
        public string Username { get; private set; } = null!;
        [Required, MaxLength(256)]
        public string PasswordHash { get; private set; } = null!;
        [Required, MaxLength(150)]
        public string Fullname { get; private set; } = null!;
        [MaxLength(20)]
        public string? PhoneNumber { get; private set; }
        [MaxLength(150)]
        public string? Email { get; private set; }
        [MaxLength(50)]
        public string? Status { get; private set; }

        private User() { }
        public static User Create(Guid roleId, string username, string passwordHash, string fullname, string? phoneNumber = null, string? email = null, string? status = null)
        {
            if (roleId == Guid.Empty) throw new ArgumentException("RoleId required");
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username required");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("PasswordHash required");
            if (string.IsNullOrWhiteSpace(fullname)) throw new ArgumentException("Fullname required");
            return new User
            {
                RoleId = roleId,
                Username = username.Trim(),
                PasswordHash = passwordHash,
                Fullname = fullname.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim(),
                Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                Status = string.IsNullOrWhiteSpace(status) ? null : status.Trim()
            };
        }

        public Role Role { get; private set; } = null!;
        public Admin? Admin { get; private set; }
        public Employee? Employee { get; private set; }
        public Customer? Customer { get; private set; }
    }
}
