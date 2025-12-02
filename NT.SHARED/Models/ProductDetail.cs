using System;
using System.Collections.Generic;

namespace NT.SHARED.Models
{
    public class ProductDetail
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ProductId { get; private set; }
        public Guid LengthId { get; private set; }
        public Guid SurfaceFinishId { get; private set; }
        public Guid HardnessId { get; private set; }
        public Guid ElasticityId { get; private set; }
        public Guid OriginCountryId { get; private set; }
        public Guid ColorId { get; private set; }
        public string? Sections { get; private set; }
        public string? CollapsedLength { get; private set; }
        public string? Weight { get; private set; }
        public string? TipWeight { get; private set; }
        public string? ButtWeight { get; private set; }
        public string? TopDiameter { get; private set; }
        public string? ButtDiameter { get; private set; }
        public string? BalancePoint { get; private set; }
        public string? BalanceLoadPoint { get; private set; }
        public string? BalanceLoadDescription { get; private set; }
        public string? RecommendedLine { get; private set; }
        public string? RecommendedFishWeight { get; private set; }
        public string? HandleType { get; private set; }
        public string? JointType { get; private set; }
        public string? Warranty { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public int SoldQuantity { get; private set; }
        public decimal? CostPrice { get; private set; }
        public decimal? ProfitMargin { get; private set; }
        public decimal? InventoryValue { get; private set; }
        public DateTime? LastImportDate { get; private set; }
        public bool IsActive { get; private set; }

        private ProductDetail() { }
        public static ProductDetail Create(Guid productId, Guid lengthId, Guid surfaceFinishId, Guid hardnessId, Guid elasticityId, Guid originCountryId, Guid colorId, decimal price, int stockQuantity)
        {
            if (productId == Guid.Empty || lengthId == Guid.Empty || surfaceFinishId == Guid.Empty || hardnessId == Guid.Empty || elasticityId == Guid.Empty || originCountryId == Guid.Empty || colorId == Guid.Empty)
                throw new ArgumentException("FKs required");
            return new ProductDetail { ProductId = productId, LengthId = lengthId, SurfaceFinishId = surfaceFinishId, HardnessId = hardnessId, ElasticityId = elasticityId, OriginCountryId = originCountryId, ColorId = colorId, Price = price, StockQuantity = stockQuantity, IsActive = true };
        }

        public Product Product { get; private set; } = null!;
        public Length Length { get; private set; } = null!;
        public SurfaceFinish SurfaceFinish { get; private set; } = null!;
        public Hardness Hardness { get; private set; } = null!;
        public Elasticity Elasticity { get; private set; } = null!;
        public OriginCountry OriginCountry { get; private set; } = null!;
        public Color Color { get; private set; } = null!;
        public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();
        public ICollection<CartDetail> CartDetails { get; private set; } = new List<CartDetail>();
        public ICollection<OrderDetail> OrderDetails { get; private set; } = new List<OrderDetail>();
    }
}
