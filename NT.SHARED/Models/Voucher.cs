using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Voucher
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(50), Display(Name = "Mã voucher")]
        public string Code { get; set; } = null!;
        [Display(Name = "Phần trăm giảm")]
        public decimal? DiscountPercentage { get; set; }
        [Display(Name = "Tiền tối đa được giảm")]
        public decimal? MaxDiscountAmount { get; set; }
        [Display(Name = "Đơn hàng tối thiểu")]
        public decimal? MinOrderAmount { get; set; }
        [Display(Name = "Bắt đầu hiệu lực")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Kết thúc hiệu lực")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Số lượng đã sử dụng")]
        public int? UsageCount { get; set; }
        [Display(Name = "Số lượng tối đa")]
        public int? MaxUsage { get; set; }

        public Voucher() { }

        // Validate all relevant properties when creating a Voucher
        public static Voucher Create(
            string code,
            decimal? discountPercentage = null,
            decimal? maxDiscountAmount = null,
            decimal? minOrderAmount = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? usageCount = null,
            int? maxUsage = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Vui lòng nhập mã voucher");

            var trimmedCode = code.Trim();
            if (trimmedCode.Length > 50)
                throw new ArgumentException("Mã voucher không được dài quá 50 ký tự");

            if (discountPercentage.HasValue && discountPercentage.Value < 0)
                throw new ArgumentException("Phần trăm giảm phải là số không âm");

            if (maxDiscountAmount.HasValue && maxDiscountAmount.Value < 0)
                throw new ArgumentException("Giảm tối đa phải là số không âm");

            if (discountPercentage.HasValue && maxDiscountAmount.HasValue && maxDiscountAmount.Value < discountPercentage.Value)
                throw new ArgumentException("Giảm tối đa phải lớn hơn hoặc bằng phần trăm giảm");

            if (minOrderAmount.HasValue && minOrderAmount.Value < 0)
                throw new ArgumentException("đơn hàng tối thiểu phải là số không âm");

            if (startDate.HasValue && endDate.HasValue && startDate.Value >= endDate.Value)
                throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            if (startDate.HasValue && startDate.Value <= DateTime.UtcNow)
                throw new ArgumentException("Ngày bắt đầu phải trong tương lai");
            if (endDate.HasValue && endDate.Value <= DateTime.UtcNow)
                throw new ArgumentException("Ngày kết thúc phải trong tương lai");

            if (usageCount.HasValue && usageCount.Value < 0)
                throw new ArgumentException("Số lượng đã sử dụng phải là số không âm");

            if (maxUsage.HasValue && maxUsage.Value < 0)
                throw new ArgumentException("Số lượng tối đa phải là số không âm");

            if (usageCount.HasValue && maxUsage.HasValue && usageCount.Value > maxUsage.Value)
                throw new ArgumentException("Số lượng đã sử dụng không thể lớn hơn số lượng tối đa");

            return new Voucher
            {
                Code = trimmedCode,
                DiscountPercentage = discountPercentage,
                MaxDiscountAmount = maxDiscountAmount,
                MinOrderAmount = minOrderAmount,
                StartDate = startDate,
                EndDate = endDate,
                UsageCount = usageCount,
                MaxUsage = maxUsage
            };
        }
    }
}
