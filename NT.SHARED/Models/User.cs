using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Vai trò")]
        public Guid RoleId { get; set; }
        [Required, MaxLength(100), Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = null!;
        [Required, MaxLength(256), Display(Name = "Mật khẩu")]
        public string PasswordHash { get; set; } = null!;
        [Required, MaxLength(150), Display(Name = "Họ và tên")]
        public string Fullname { get; set; } = null!;
        [MaxLength(20), Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }
        [MaxLength(150), Display(Name = "Email")]
        public string? Email { get; set; }
        [MaxLength(50), Display(Name = "Trạng thái")]
        public string? Status { get; set; }

        public User() { }
        public static User Create(Guid roleId, string username, string passwordHash, string fullname, string? phoneNumber = null, string? email = null, string? status = null)
        {
            if (roleId == Guid.Empty) throw new ArgumentException("Vui lòng cung cấp Id vai trò");
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Vui lòng nhập tên đăng nhập");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Vui lòng cung cấp mật khẩu");
            if (string.IsNullOrWhiteSpace(fullname)) throw new ArgumentException("Vui lòng nhập họ và tên");
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

        public Role? Role { get; set; }
        public Admin? Admin { get; set; }
        public Employee? Employee { get; set; }
        public Customer? Customer { get; set; }
    }
}
