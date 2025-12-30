using System;

namespace NT.WEB.DTO
{
    public class CartItemDto
    {
        public Guid ProductDetailId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? Thumbnail { get; set; }
        public string? LengthName { get; set; }
        public string? HardnessName { get; set; }
        public string? ColorName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
