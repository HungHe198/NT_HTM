using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class OrderDetail
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // Foreign key tới Order
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; } = null!;

        // Foreign key tới ProductDetail
        public Guid ProductDetailId { get; private set; }
        public ProductDetail ProductDetail { get; private set; } = null!;

        [Required]
        public int Quantity { get; private set; }

        [Required]
        public decimal UnitPrice { get; private set; }

        public decimal UnitName { get; private set; }
        public decimal UnitColor { get; private set; }
        public decimal UnitHardness { get; private set; }
        public decimal UnitLength { get; private set; }

        private OrderDetail() { }

        public static OrderDetail Create(Guid id, Guid orderId, Guid productDetailId, int qty, decimal price)
        {
            return new OrderDetail
            {
                Id = id,
                OrderId = orderId,
                ProductDetailId = productDetailId,
                Quantity = qty,
                UnitPrice = price
            };
        }
    }
}
