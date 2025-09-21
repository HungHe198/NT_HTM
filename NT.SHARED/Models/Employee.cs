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
            return new Employee { Id = userId, FullName = fullName, PhoneNumber = phone, Salary = salary };
        }

        public User User { get; private set; } = null!;
    }
}
