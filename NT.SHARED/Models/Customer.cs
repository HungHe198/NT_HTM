using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Customer
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string FullName { get; private set; } = null!;
        [Required, MaxLength(200)]
        public string Address { get; private set; } = null!;
        [Required, MaxLength(20)]
        public string PhoneNumber { get; private set; } = null!;
        public Guid UserId { get; private set; } 

        private Customer() { }

        public static Customer Create(Guid userId, string fullName, string address, string phone)
        {
            return new Customer { Id = userId, FullName = fullName, Address = address, PhoneNumber = phone };
        }

        public User User { get; private set; } = null!;
        public ICollection<Order> Orders { get; private set; } = new List<Order>();
    }
}
