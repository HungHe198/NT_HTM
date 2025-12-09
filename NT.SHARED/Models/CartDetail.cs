using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class CartDetail
    {
        [Display(Name = "Giỏ hàng của")]
        public Guid CartId { get; set; }
        [Display(Name = "Mã sản phẩm đã chọn")]
        public Guid ProductDetailId { get; set; }
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        public CartDetail() { }
        public static CartDetail Create(Guid cartId, Guid productDetailId, int quantity)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) throw new ArgumentException("Vui lòng đăng nhập để thực hiện tính năng");
            if (quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
            return new CartDetail { CartId = cartId, ProductDetailId = productDetailId, Quantity = quantity };
        }

        public Cart? Cart { get; set; } 
        public ProductDetail? ProductDetail { get; set; } 
    }
}
