using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Employee
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Display(Name = "Tên nhân viên")]
        public Guid UserId { get; private set; }
        [Display(Name = "Chức vụ")]
        public string? Position { get; private set; }
        [Display(Name = "Lương")]
        public decimal? Salary { get; private set; }

        private Employee() { }
        public static Employee Create(Guid userId, string? position = null, decimal? salary = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("Vui lòng đăng nhập lại để thực hiện chức năng này(102)");
            return new Employee { UserId = userId, Position = string.IsNullOrWhiteSpace(position) ? null : position.Trim(), Salary = salary };
        }

        public User User { get; private set; } = null!;
        
    }
}
