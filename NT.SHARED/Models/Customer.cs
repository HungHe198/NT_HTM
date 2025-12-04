using System;

namespace NT.SHARED.Models
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string? Address { get; set; }
        public DateTime? DoB { get; set; }
        public string? Gender { get; set; }

        public Customer() { }
        public static Customer Create(Guid userId, string? address = null, DateTime? dob = null, string? gender = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId required");
            return new Customer { UserId = userId, Address = address, DoB = dob, Gender = gender };
        }

        public User User { get; set; } = null!;
        public Cart? Cart { get; set; }
    }
}
