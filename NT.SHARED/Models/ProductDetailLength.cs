using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class ProductDetailLength
    {
        public Guid ProductDetailId { get; private set; }
        public Guid RodLengthId { get; private set; }

        private ProductDetailLength() { }

        public static ProductDetailLength Create(Guid detailId, Guid lengthId)
        {
            return new ProductDetailLength { ProductDetailId = detailId, RodLengthId = lengthId };
        }

        public ProductDetail ProductDetail { get; private set; } = null!;
        public RodLength RodLength { get; private set; } = null!;
    }
}
