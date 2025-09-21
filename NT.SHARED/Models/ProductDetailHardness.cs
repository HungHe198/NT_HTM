using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class ProductDetailHardness
    {
        public Guid ProductDetailId { get; private set; }
        public Guid RodHardnessId { get; private set; }

        private ProductDetailHardness() { }

        public static ProductDetailHardness Create(Guid detailId, Guid hardnessId)
        {
            return new ProductDetailHardness { ProductDetailId = detailId, RodHardnessId = hardnessId };
        }

        public ProductDetail ProductDetail { get; private set; } = null!;
        public RodHardness RodHardness { get; private set; } = null!;
    }
}
