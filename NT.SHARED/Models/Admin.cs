using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Admin
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Display(Name = "Tên Admin")]
        public Guid UserId { get; private set; }
        [MaxLength(100), Display(Name = "Chức vụ")]
        public string? Position { get; private set; }
        [Display(Name = "Lương")]
        public decimal? Salary { get; private set; }

        private Admin() { }
        public static Admin Create(Guid userId, string? position = null, decimal? salary = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("Vui lòng đăng nhập lại để thực hiện chức năng này(101)");
            return new Admin { UserId = userId, Position = string.IsNullOrWhiteSpace(position) ? null : position.Trim(), Salary = salary };
        }

        public User? User { get; private set; } 
    }
}
