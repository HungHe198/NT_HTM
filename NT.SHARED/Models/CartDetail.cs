using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    // Ch?n theo ProductDetail ?? l?u ?úng bi?n th? (màu/?? c?ng/chi?u dài) ng??i dùng thêm vào gi?
    public class CartDetail
    {
        public Guid CartId { get; private set; }
        public Guid ProductDetailId { get; private set; }

        [Required]
        public int Quantity { get; private set; }
        // Snapshot giá t?i th?i ?i?m thêm vào gi? (?? hi?n th? ?úng, ??n hàng s? ch?t l?i UnitPrice khi checkout)
        [Required]
        public decimal PriceAtAdd { get; private set; }

        private CartDetail() { }

        public static CartDetail Create(Guid cartId, Guid productDetailId, int quantity, decimal priceAtAdd)
        {
            if (cartId == Guid.Empty) throw new ArgumentException("CartId không h?p l?");
            if (productDetailId == Guid.Empty) throw new ArgumentException("ProductDetailId không h?p l?");
            if (quantity <= 0) throw new ArgumentException("S? l??ng ph?i > 0");
            if (priceAtAdd < 0) throw new ArgumentException("Giá không h?p l?");
            return new CartDetail { CartId = cartId, ProductDetailId = productDetailId, Quantity = quantity, PriceAtAdd = priceAtAdd };
        }

        public Cart Cart { get; private set; } = null!;
        public ProductDetail ProductDetail { get; private set; } = null!;
    }
}
