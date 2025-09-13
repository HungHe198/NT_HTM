using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class RodColor
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Name { get; private set; } = null!; // Tên màu sắc (VD: Đen)
        [MaxLength(200)]
        public string? Description { get; private set; } // Mô tả màu sắc

        // Private constructor for EF
        private RodColor() { }

        // Public static factory
        public static RodColor Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên màu sắc không được để trống", nameof(name));
            return new RodColor
            {
                Name = name,
                Description = description
            };
        }

        // Navigation
        public ICollection<ProductDetail> ProductDetails { get; private set; } = new List<ProductDetail>();
    }
}