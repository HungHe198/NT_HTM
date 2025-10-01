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
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên thương hiệu không được để trống");
            return new Brand { Name = name.Trim(), Website = string.IsNullOrWhiteSpace(website) ? null : website };
        }

        public ICollection<Product> Products { get; private set; } = new List<Product>();
    }
}
