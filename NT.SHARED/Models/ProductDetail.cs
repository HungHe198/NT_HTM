using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class ProductDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // ===== KHÓA NGOẠI (BẮT BUỘC) =====
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chiều dài cần")]
        [Display(Name = "Chiều dài cần (m)")]
        public Guid LengthId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hoàn thiện bề mặt")]
        [Display(Name = "Hoàn thiện bề mặt")]
        public Guid SurfaceFinishId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn độ cứng")]
        [Display(Name = "Độ cứng (H)")]
        public Guid HardnessId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn độ đàn hồi")]
        [Display(Name = "Độ đàn hồi")]
        public Guid ElasticityId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn xuất xứ")]
        [Display(Name = "Xuất xứ")]
        public Guid OriginCountryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn màu sắc")]
        [Display(Name = "Màu sắc")]
        public Guid ColorId { get; set; }

        // ===== THÔNG SỐ KỸ THUẬT CẦN CÂU ĐÀI =====
        [Range(1, 20, ErrorMessage = "Số lóng cần phải từ 1 đến 20 đốt")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số lóng cần phải là số nguyên")]
        [MaxLength(10, ErrorMessage = "Số lóng cần tối đa 10 ký tự")]
        [Display(Name = "Số lóng cần (đốt)")]
        public string? Sections { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Chiều dài gập gọn phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Chiều dài gập gọn tối đa 20 ký tự")]
        [Display(Name = "Chiều dài gập gọn (cm)")]
        public string? CollapsedLength { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Trọng lượng cần phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Trọng lượng cần tối đa 20 ký tự")]
        [Display(Name = "Trọng lượng cần (g)")]
        public string? Weight { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Trọng lượng đầu phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Trọng lượng đầu tối đa 20 ký tự")]
        [Display(Name = "Trọng lượng đầu (g)")]
        public string? TipWeight { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Trọng lượng cán phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Trọng lượng cán tối đa 20 ký tự")]
        [Display(Name = "Trọng lượng cán (g)")]
        public string? ButtWeight { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Đường kính ngọn cần phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Đường kính ngọn cần tối đa 20 ký tự")]
        [Display(Name = "Đường kính ngọn cần - lóng 1 (mm)")]
        public string? TipDiameter { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Đường kính đầu phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Đường kính đầu tối đa 20 ký tự")]
        [Display(Name = "Đường kính đầu - lóng cuối (mm)")]
        public string? TopDiameter { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Đường kính cán phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Đường kính cán tối đa 20 ký tự")]
        [Display(Name = "Đường kính cán - đuôi lóng cuối (mm)")]
        public string? ButtDiameter { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điểm cân bằng tĩnh phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Điểm cân bằng tĩnh tối đa 20 ký tự")]
        [Display(Name = "Điểm cân bằng tĩnh (cm từ đuôi)")]
        public string? BalancePoint { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điểm tải cân bằng phải là số (tối đa 2 chữ số thập phân)")]
        [MaxLength(20, ErrorMessage = "Điểm tải cân bằng tối đa 20 ký tự")]
        [Display(Name = "Điểm tải cân bằng khi chịu tải (cm)")]
        public string? BalanceLoadPoint { get; set; }

        [MaxLength(500, ErrorMessage = "Mô tả tải cân bằng tối đa 500 ký tự")]
        [Display(Name = "Mô tả tải cân bằng")]
        public string? BalanceLoadDescription { get; set; }

        [MaxLength(100, ErrorMessage = "Dây câu đề xuất tối đa 100 ký tự")]
        [Display(Name = "Dây câu đề xuất (số hiệu)")]
        public string? RecommendedLine { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?(-\d+(\.\d{1,2})?)?$", ErrorMessage = "Trọng lượng cá đề xuất phải là số hoặc khoảng (VD: 2.5 hoặc 1.5-3)")]
        [MaxLength(50, ErrorMessage = "Trọng lượng cá đề xuất tối đa 50 ký tự")]
        [Display(Name = "Trọng lượng cá đề xuất (kg)")]
        public string? RecommendedFishWeight { get; set; }

        [MaxLength(100, ErrorMessage = "Loại tay cầm tối đa 100 ký tự")]
        [Display(Name = "Loại tay cầm")]
        public string? HandleType { get; set; }

        [MaxLength(100, ErrorMessage = "Loại khớp nối tối đa 100 ký tự")]
        [Display(Name = "Loại khớp nối")]
        public string? JointType { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Bảo hành phải là số nguyên (tháng)")]
        [MaxLength(10, ErrorMessage = "Bảo hành tối đa 10 ký tự")]
        [Display(Name = "Bảo hành (tháng)")]
        public string? Warranty { get; set; }

        // ===== GIÁ CẢ VÀ TỒN KHO =====
        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        [Range(0, 999999999999, ErrorMessage = "Giá bán phải từ 0 đến 999,999,999,999 VNĐ")]
        [Display(Name = "Giá bán (VNĐ)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn kho")]
        [Range(0, 999999, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 999,999")]
        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; }

        [Range(0, 999999, ErrorMessage = "Số lượng đã bán phải từ 0 đến 999,999")]
        [Display(Name = "Số lượng đã bán")]
        public int SoldQuantity { get; set; }

        [Range(0, 999999999999, ErrorMessage = "Giá vốn phải từ 0 đến 999,999,999,999 VNĐ")]
        [Display(Name = "Giá vốn (VNĐ)")]
        public decimal? CostPrice { get; set; }

        [DataType(DataType.Date)]
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

        
        public Product? Product { get; set; }
        
        public Length? Length { get; set; }
        
        public SurfaceFinish? SurfaceFinish { get; set; }
        
        public Hardness? Hardness { get; set; }
        
        public Elasticity? Elasticity { get; set; }
      
        public OriginCountry? OriginCountry { get; set; }
    
        public Color? Color { get; set; }
        public ICollection<ProductImage>? Images { get; set; } = new List<ProductImage>();
        public ICollection<CartDetail>? CartDetails { get; set; } = new List<CartDetail>();
        public ICollection<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
