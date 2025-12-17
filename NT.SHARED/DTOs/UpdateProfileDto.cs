using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.DTOs
{
    /// <summary>
    /// DTO cho việc cập nhật thông tin cá nhân của người dùng.
    /// Chỉ bao gồm các trường được phép chỉnh sửa.
    /// Không bao gồm: Id, RoleId, Username (không được thay đổi)
    /// </summary>
    public class UpdateProfileDto
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [MaxLength(150, ErrorMessage = "Họ và tên không được vượt quá 150 ký tự")]
        [Display(Name = "Họ và tên")]
        public string Fullname { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [MaxLength(150, ErrorMessage = "Email không được vượt quá 150 ký tự")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        // Thông tin Customer (nếu là khách hàng)
        [MaxLength(300, ErrorMessage = "Địa chỉ không được vượt quá 300 ký tự")]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DoB { get; set; }

        [MaxLength(20, ErrorMessage = "Giới tính không được vượt quá 20 ký tự")]
        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }
    }
}
