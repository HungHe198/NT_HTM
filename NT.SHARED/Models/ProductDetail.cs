using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class ProductDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Tên Sản phẩm")]
        public Guid ProductId { get; set; }
        [Display(Name = "Chiều dài")]
        public Guid LengthId { get; set; }
        [Display(Name = "Chất liệu hoàn thiện bề mặt")]
        public Guid SurfaceFinishId { get; set; }
        [Display(Name = "Độ cứng")]
        public Guid HardnessId { get; set; }
        [Display(Name = "Độ đàn hồi")]
        public Guid ElasticityId { get; set; }
        [Display(Name = "Xuất xứ(Thuộc nước?)")]
        public Guid OriginCountryId { get; set; }
        [Display(Name = "Màu")]
        public Guid ColorId { get; set; }
        [Display(Name = "Số lóng cần")]
        public string? Sections { get; set; }
        [Display(Name = "Chiều dài gập gọn")]
        public string? CollapsedLength { get; set; }
        [Display(Name = "Trọng lượng cần")]
        public string? Weight { get; set; }
        [Display(Name = "Trọng lượng đầu")]
        public string? TipWeight { get; set; }
        [Display(Name = "Trọng lượng cán")]
        public string? ButtWeight { get; set; }
        [Display(Name = "Đường kính ngọn cần(Đường kính của đầu ngọn lóng 1)")]
        public string? TipDiameter { get; set; }
        [Display(Name = "Đường kính đầu(Đường kính của đầu lóng cuối cùng)")]
        public string? TopDiameter { get; set; }
        [Display(Name = "Đường kính cán(Đường kính của đuôi lóng cuối cùng)")]
        public string? ButtDiameter { get; set; }
        [Display(Name = "Điểm cân bằng tĩnh(Trung tâm khối lượng của cần ở trạng thái không mang tải trọng! )")]
        public string? BalancePoint { get; set; }
        [Display(Name = "Điểm tải cân bằng(khi chịu tải)")]
        public string? BalanceLoadPoint { get; set; }
        [Display(Name = "Mô tả tải cân bằng")]
        public string? BalanceLoadDescription { get; set; }
        [Display(Name = "Dây câu đề xuất")]
        public string? RecommendedLine { get; set; }
        [Display(Name = "Trọng lượng cá đề xuất")]
        public string? RecommendedFishWeight { get; set; }
        [Display(Name = "Loại tay cầm")]
        public string? HandleType { get; set; }
        [Display(Name = "Loại khớp nối")]
        public string? JointType { get; set; }
        [Display(Name = "Bảo hành")]
        public string? Warranty { get; set; }
        [Display(Name = "Giá")]
        public decimal Price { get; set; }
        [Display(Name = "Số lượng tồn")]
        public int StockQuantity { get; set; }
        [Display(Name = "Số lượng bán")]
        public int SoldQuantity { get; set; }
        [Display(Name = "Giá vốn")]
        public decimal? CostPrice { get; set; }  
        [Display(Name = "Ngày nhập cuối")]
        public DateTime? LastImportDate { get; set; }
        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; }

        public ProductDetail() { }
        public static ProductDetail Create(Guid productId, Guid lengthId, Guid surfaceFinishId, Guid hardnessId, Guid elasticityId, Guid originCountryId, Guid colorId, decimal price, int stockQuantity)
        {
            if (productId == Guid.Empty || lengthId == Guid.Empty || surfaceFinishId == Guid.Empty || hardnessId == Guid.Empty || elasticityId == Guid.Empty || originCountryId == Guid.Empty || colorId == Guid.Empty)
                throw new ArgumentException("Vui lòng cung cấp đầy đủ khóa ngoại: ProductId, LengthId, SurfaceFinishId, HardnessId, ElasticityId, OriginCountryId, ColorId");
            return new ProductDetail { ProductId = productId, LengthId = lengthId, SurfaceFinishId = surfaceFinishId, HardnessId = hardnessId, ElasticityId = elasticityId, OriginCountryId = originCountryId, ColorId = colorId, Price = price, StockQuantity = stockQuantity, IsActive = true };
        }

        [Display(Name = "Sản phẩm")]
        public Product Product { get; set; } = null!;
        [Display(Name = "Chiều dài")]
        public Length Length { get; set; } = null!;
        [Display(Name = "Hoàn thiện bề mặt")]
        public SurfaceFinish SurfaceFinish { get; set; } = null!;
        [Display(Name = "Độ cứng")]
        public Hardness Hardness { get; set; } = null!;
        [Display(Name = "Độ đàn hồi")]
        public Elasticity Elasticity { get; set; } = null!;
        [Display(Name = "Xuất xứ")]
        public OriginCountry OriginCountry { get; set; } = null!;
        [Display(Name = "Màu")]
        public Color Color { get; set; } = null!;
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
