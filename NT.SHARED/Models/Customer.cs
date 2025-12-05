using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Tên khách hàng")]
        public Guid UserId { get; set; }
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }
        [Display(Name = "Ngày sinh")]
        public DateTime? DoB { get; set; }
        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }

        public Customer() { }
        public static Customer Create(Guid userId, string? address = null, DateTime? dob = null, string? gender = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("Vui lòng tiến hành đăng nhập lại để thực hiện chức năng này!(103)");
            return new Customer { UserId = userId, Address = address, DoB = dob, Gender = gender };
        }

        public User User { get; set; } = null!;
        public Cart? Cart { get; set; }
    }
}
