using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Admin
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        [MaxLength(100)]
        public string? Position { get; private set; }
        public decimal? Salary { get; private set; }

        private Admin() { }
        public static Admin Create(Guid userId, string? position = null, decimal? salary = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId required");
            return new Admin { UserId = userId, Position = string.IsNullOrWhiteSpace(position) ? null : position.Trim(), Salary = salary };
        }

        public User User { get; private set; } = null!;
    }
}
