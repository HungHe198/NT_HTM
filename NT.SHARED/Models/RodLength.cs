using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class RodLength
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required]
        public decimal Value { get; private set; } // Độ dài (VD: 360)
        [Required, MaxLength(10)]
        public string Unit { get; private set; } = "cm"; // Đơn vị (mặc định cm)

        // Private constructor for EF
        private RodLength() { }

        // Public static factory
        public static RodLength Create(decimal value, string unit = "cm")
        {
            if (value <= 0) throw new ArgumentException("Độ dài phải lớn hơn 0", nameof(value));
            if (string.IsNullOrWhiteSpace(unit)) throw new ArgumentException("Đơn vị không được để trống", nameof(unit));
            return new RodLength
            {
                Value = value,
                Unit = unit
            };
        }

        // Navigation
        public ICollection<ProductDetail> ProductDetails { get; private set; } = new List<ProductDetail>();
    }
}