using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class ProductCategory : IValidatableObject
    {
        [Display(Name = "Danh mục")]
        public Guid CategoryId { get; set; }
        [Display(Name = "Sản phẩm")]
        public Guid ProductId { get; set; }

        public ProductCategory() { }
        public static ProductCategory Create(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) throw new ArgumentException("danh mục hoặc sản phẩm không hợp lệ");
            return new ProductCategory { CategoryId = categoryId, ProductId = productId };
        }

        // Validate composite key values for model binding and server-side checks
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CategoryId == Guid.Empty)
            {
                yield return new ValidationResult("Vui lòng chọn danh mục hợp lệ.", new[] { nameof(CategoryId) });
            }

            if (ProductId == Guid.Empty)
            {
                yield return new ValidationResult("Vui lòng chọn sản phẩm hợp lệ.", new[] { nameof(ProductId) });
            }

            // optional: ensure product and category are not the same id (business rule rarely required)
            if (CategoryId != Guid.Empty && ProductId != Guid.Empty && CategoryId == ProductId)
            {
                yield return new ValidationResult("Mã danh mục và mã sản phẩm không thể giống nhau.", new[] { nameof(CategoryId), nameof(ProductId) });
            }
        }

        public Category? Category { get; set; }
        public Product? Product { get; set; }
    }
}
