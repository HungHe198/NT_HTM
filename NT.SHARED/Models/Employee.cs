using System;

namespace NT.SHARED.Models
{
    public class Employee
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public Guid? ManagerId { get; private set; }
        public string? Position { get; private set; }
        public decimal? Salary { get; private set; }

        private Employee() { }
        public static Employee Create(Guid userId, Guid? managerId = null, string? position = null, decimal? salary = null)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId required");
            return new Employee { UserId = userId, ManagerId = managerId, Position = position, Salary = salary };
        }

        public User User { get; private set; } = null!;
        public Employee? Manager { get; private set; }
    }
}
