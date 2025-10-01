using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Employee
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string FullName { get; private set; } = null!;
        [Required, MaxLength(20)]
        public string PhoneNumber { get; private set; } = null!;
        [Required]
        public decimal Salary { get; private set; }
        public Guid UserId { get; private set; }

        private Employee() { }

        public static Employee Create(Guid userId, string fullName, string phone, decimal salary)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId không hợp lệ");
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName không được để trống");
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("PhoneNumber không được để trống");
            if (salary < 0) throw new ArgumentException("Salary không hợp lệ");
            return new Employee { UserId = userId, FullName = fullName, PhoneNumber = phone, Salary = salary };
        }

        public User User { get; private set; } = null!;
    }
}
