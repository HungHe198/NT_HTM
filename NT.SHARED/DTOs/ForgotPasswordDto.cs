using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.DTOs
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên người dùng không được vượt quá 100 ký tự")]
        public string Username { get; set; } = null!; // Để tìm user

        [Required(ErrorMessage = "Email là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        public string Email { get; set; } = null!; // Để gửi link reset password
    }
}
