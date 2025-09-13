using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class ActorType
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Name { get; private set; } = null!; // Admin, Customer, Employee

        // Private constructor for EF
        private ActorType() { }

        // Public static factory
        public static ActorType Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Không được để trống tên Actor", nameof(name));
            return new ActorType { Name = name };
        }

        // Navigation
        public ICollection<User> Users { get; private set; } = new List<User>();
        public ICollection<Role> Roles { get; private set; } = new List<Role>();
    }
}