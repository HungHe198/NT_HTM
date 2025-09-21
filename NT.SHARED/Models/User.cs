using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Username { get; private set; } = null!;
        [Required]
        public string PasswordHash { get; private set; } = null!;
        [Required]
        public bool Status { get; private set; } 
        [Required]
        public Guid RoleId { get; private set; }

        private User() { }

        public static User Create(string username, string passwordHash, bool status, Guid roleId)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Không được để trống username");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Không được để trống password");
            if (string.IsNullOrWhiteSpace(status.ToString())) throw new ArgumentException("Trạng thái không hợp lệ");
            if (roleId == Guid.Empty) throw new ArgumentException("RoleId không hợp lệ");

            return new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Status = status,
                RoleId = roleId
            };
        }

        // Navigation
        public Role Role { get; private set; } = null!;
        public ICollection<Order> Orders { get; private set; } = new List<Order>();
        public Admin? Admin { get; private set; }
        public Employee? Employee { get; private set; }
        public Customer? Customer { get; private set; }
    }
}
