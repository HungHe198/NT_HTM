using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class ProductDetailColor
    {
        public Guid ProductDetailId { get; private set; }
        public Guid RodColorId { get; private set; }

        private ProductDetailColor() { }

        public static ProductDetailColor Create(Guid detailId, Guid colorId)
        {
            return new ProductDetailColor { ProductDetailId = detailId, RodColorId = colorId };
        }

        public ProductDetail ProductDetail { get; private set; } = null!;
        public RodColor RodColor { get; private set; } = null!;
    }
}
