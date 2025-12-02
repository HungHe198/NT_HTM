using System;

namespace NT.SHARED.Models
{
    public class CartDetail
    {
        public Guid CartId { get; private set; }
        public Guid ProductDetailId { get; private set; }
        public int Quantity { get; private set; }

        private CartDetail() { }
        public static CartDetail Create(Guid cartId, Guid productDetailId, int quantity)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) throw new ArgumentException("Invalid ids");
            if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");
            return new CartDetail { CartId = cartId, ProductDetailId = productDetailId, Quantity = quantity };
        }

        public Cart Cart { get; private set; } = null!;
        public ProductDetail ProductDetail { get; private set; } = null!;
    }
}
