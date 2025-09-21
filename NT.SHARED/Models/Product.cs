using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(200)]
        public string Name { get; private set; } = null!;
        [Required]
        public Guid BrandId { get; private set; }

        private Product() { }

        public static Product Create(string name, Guid brandId)
        {
            return new Product { Name = name, BrandId = brandId };
        }

        public Brand Brand { get; private set; } = null!;
        public ICollection<ProductDetail> ProductDetails { get; private set; } = new List<ProductDetail>();
        public ICollection<ProductCategory> ProductCategories { get; private set; } = new List<ProductCategory>();
    }
}