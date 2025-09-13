using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class RodHardness
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Name { get; private set; } = null!; // Tên độ cứng (VD: 4H)
        [MaxLength(200)]
        public string? Description { get; private set; } // Mô tả độ cứng

        // Private constructor for EF
        private RodHardness() { }

        // Public static factory
        public static RodHardness Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên độ cứng không được để trống", nameof(name));
            return new RodHardness
            {
                Name = name,
                Description = description
            };
        }

        // Navigation
        public ICollection<ProductDetail> ProductDetails { get; private set; } = new List<ProductDetail>();
    }
}