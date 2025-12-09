using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên người dùng không được vượt quá 100 ký tự")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự")]
        public string Password { get; set; } = null!; // Plain text, sẽ verify với PasswordHash

        // Thêm thuộc tính RememberMe để dùng cho đăng nhập duy trì phiên (persistent cookie)
        public bool RememberMe { get; set; }
    }
}
