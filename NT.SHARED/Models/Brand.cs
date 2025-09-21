using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Brand
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(150)]
        public string Name { get; private set; } = null!;
        [MaxLength(200)]
        public string? Website { get; private set; }

        private Brand() { }

        public static Brand Create(string name, string? website = null)
        {
            return new Brand { Name = name, Website = website };
        }

        public ICollection<Product> Products { get; private set; } = new List<Product>();
    }
}
