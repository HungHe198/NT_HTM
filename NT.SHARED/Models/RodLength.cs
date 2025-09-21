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
        public double Value { get; private set; }

        private RodLength() { }

        public static RodLength Create(double value) => new RodLength { Value = value };

        public ICollection<ProductDetailLength> ProductDetailLengths { get; private set; } = new List<ProductDetailLength>();
    }
}