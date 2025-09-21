using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class ProductDetail
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required]
        public Guid ProductId { get; private set; }
        [Required]
        public decimal Price { get; private set; }
        [Required]
        public int Stock { get; private set; }

        private ProductDetail() { }

        public static ProductDetail Create(Guid productId, decimal price, int stock)
        {
            return new ProductDetail { ProductId = productId, Price = price, Stock = stock };
        }

        public Product Product { get; private set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; private set; } = new List<OrderDetail>();
        public ICollection<ProductDetailColor> ProductDetailColors { get; private set; } = new List<ProductDetailColor>();
        public ICollection<ProductDetailHardness> ProductDetailHardnesses { get; private set; } = new List<ProductDetailHardness>();
        public ICollection<ProductDetailLength> ProductDetailLengths { get; private set; } = new List<ProductDetailLength>();
    }
}