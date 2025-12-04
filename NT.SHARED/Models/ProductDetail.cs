using System;
using System.Collections.Generic;

namespace NT.SHARED.Models
{
    public class ProductDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid LengthId { get; set; }
        public Guid SurfaceFinishId { get; set; }
        public Guid HardnessId { get; set; }
        public Guid ElasticityId { get; set; }
        public Guid OriginCountryId { get; set; }
        public Guid ColorId { get; set; }
        public string? Sections { get; set; }
        public string? CollapsedLength { get; set; }
        public string? Weight { get; set; }
        public string? TipWeight { get; set; }
        public string? ButtWeight { get; set; }
        public string? TopDiameter { get; set; }
        public string? ButtDiameter { get; set; }
        public string? BalancePoint { get; set; }
        public string? BalanceLoadPoint { get; set; }
        public string? BalanceLoadDescription { get; set; }
        public string? RecommendedLine { get; set; }
        public string? RecommendedFishWeight { get; set; }
        public string? HandleType { get; set; }
        public string? JointType { get; set; }
        public string? Warranty { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? ProfitMargin { get; set; }
        public decimal? InventoryValue { get; set; }
        public DateTime? LastImportDate { get; set; }
        public bool IsActive { get; set; }

        public ProductDetail() { }
        public static ProductDetail Create(Guid productId, Guid lengthId, Guid surfaceFinishId, Guid hardnessId, Guid elasticityId, Guid originCountryId, Guid colorId, decimal price, int stockQuantity)
        {
            if (productId == Guid.Empty || lengthId == Guid.Empty || surfaceFinishId == Guid.Empty || hardnessId == Guid.Empty || elasticityId == Guid.Empty || originCountryId == Guid.Empty || colorId == Guid.Empty)
                throw new ArgumentException("FKs required");
            return new ProductDetail { ProductId = productId, LengthId = lengthId, SurfaceFinishId = surfaceFinishId, HardnessId = hardnessId, ElasticityId = elasticityId, OriginCountryId = originCountryId, ColorId = colorId, Price = price, StockQuantity = stockQuantity, IsActive = true };
        }

        public Product Product { get; set; } = null!;
        public Length Length { get; set; } = null!;
        public SurfaceFinish SurfaceFinish { get; set; } = null!;
        public Hardness Hardness { get; set; } = null!;
        public Elasticity Elasticity { get; set; } = null!;
        public OriginCountry OriginCountry { get; set; } = null!;
        public Color Color { get; set; } = null!;
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
