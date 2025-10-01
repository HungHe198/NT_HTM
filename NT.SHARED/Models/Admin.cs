using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Admin
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string FullName { get; private set; } = null!;
        [Required, MaxLength(20)]
        public string PhoneNumber { get; private set; } = null!;
        public Guid UserId { get; private set; } 

        private Admin() { }

        public static Admin Create(Guid userId, string fullName, string phoneNumber)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId không hợp lệ");
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName không được để trống");
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("PhoneNumber không được để trống");
            return new Admin { UserId = userId, FullName = fullName, PhoneNumber = phoneNumber };
        }

        public User User { get; private set; } = null!;
    }
}
