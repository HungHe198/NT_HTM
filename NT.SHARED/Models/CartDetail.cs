using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    // Ch?n theo ProductDetail ?? l?u ?�ng bi?n th? (m�u/?? c?ng/chi?u d�i) ng??i d�ng th�m v�o gi?
    public class CartDetail
    {
        public Guid CartId { get; private set; }
        public Guid ProductDetailId { get; private set; }

        [Required]
        public int Quantity { get; private set; }
        // Snapshot gi� t?i th?i ?i?m th�m v�o gi? (?? hi?n th? ?�ng, ??n h�ng s? ch?t l?i UnitPrice khi checkout)
        [Required]
        public decimal PriceAtAdd { get; private set; }

        private CartDetail() { }

        public static CartDetail Create(Guid cartId, Guid productDetailId, int quantity, decimal priceAtAdd)
        {
            if (cartId == Guid.Empty) throw new ArgumentException("CartId kh�ng h?p l?");
            if (productDetailId == Guid.Empty) throw new ArgumentException("ProductDetailId kh�ng h?p l?");
            if (quantity <= 0) throw new ArgumentException("S? l??ng ph?i > 0");
            if (priceAtAdd < 0) throw new ArgumentException("Gi� kh�ng h?p l?");
            return new CartDetail { CartId = cartId, ProductDetailId = productDetailId, Quantity = quantity, PriceAtAdd = priceAtAdd };
        }

        public Cart Cart { get; private set; } = null!;
        public ProductDetail ProductDetail { get; private set; } = null!;
    }
}
