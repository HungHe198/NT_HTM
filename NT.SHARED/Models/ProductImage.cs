using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Ảnh của biến thể")]
        public Guid ProductDetailId { get; set; }
        [Display(Name = "Đường dẫn ảnh"), MaxLength(300)]
        public string ImageUrl { get; set; } = null!;

        public ProductImage() { }
        public static ProductImage Create(Guid productDetailId, string imageUrl)
        {
            if (productDetailId == Guid.Empty) throw new ArgumentException("Vui lòng chọn biến thể hợp lệ");
            if (string.IsNullOrWhiteSpace(imageUrl)) throw new ArgumentException("Vui lòng điền đường dẫn ảnh hợp lệ");
            return new ProductImage { ProductDetailId = productDetailId, ImageUrl = imageUrl.Trim() };
        }

        public ProductDetail? ProductDetail { get; set; }   
    }
}
