using System;

namespace NT.SHARED.Models
{
    public class Customer
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public string? Address { get; private set; }
        public DateTime? DoB { get; private set; }
        public string? Gender { get; private set; }

        private Customer() { }
        public static Customer Create(Guid userId, string? address = null, DateTime? dob = null, string? gender = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId required");
            return new Customer { UserId = userId, Address = address, DoB = dob, Gender = gender };
        }

        public User User { get; private set; } = null!;
        public Cart? Cart { get; private set; }
    }
}
