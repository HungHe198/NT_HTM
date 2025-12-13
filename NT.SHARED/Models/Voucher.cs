using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Voucher
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(50), Display(Name = "Mã voucher")]
        public string Code { get; set; } = null!;
        [Display(Name = "Số tiền giảm")]
        public decimal? DiscountAmount { get; set; }
        [Display(Name = "Tiền tối đa được giảm")]
        public decimal? MaxDiscountAmount { get; set; }
        [Display(Name = "đơn hàng tối thiểu")]
        public decimal? MinOrderAmount { get; set; }
        [Display(Name = "Hạn sử dụng")]
        public DateTime? ExpiryDate { get; set; }
        [Display(Name = "Số lượng đã sử dụng")]
        public int? UsageCount { get; set; }
        [Display(Name = "Số lượng tối đa")]
        public int? MaxUsage { get; set; }

        public Voucher() { }

        // Validate all relevant properties when creating a Voucher
        public static Voucher Create(
            string code,
            decimal? discountAmount = null,
            decimal? maxDiscountAmount = null,
            decimal? minOrderAmount = null,
            DateTime? expiryDate = null,
            int? usageCount = null,
            int? maxUsage = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Vui lòng nhập mã voucher");

            var trimmedCode = code.Trim();
            if (trimmedCode.Length > 50)
                throw new ArgumentException("Mã voucher không được dài quá 50 ký tự");

            if (discountAmount.HasValue && discountAmount.Value < 0)
                throw new ArgumentException("Số tiền giảm phải là số không âm");

            if (maxDiscountAmount.HasValue && maxDiscountAmount.Value < 0)
                throw new ArgumentException("Giảm tối đa phải là số không âm");

            if (discountAmount.HasValue && maxDiscountAmount.HasValue && maxDiscountAmount.Value < discountAmount.Value)
                throw new ArgumentException("Giảm tối đa phải lớn hơn hoặc bằng số tiền giảm");

            if (minOrderAmount.HasValue && minOrderAmount.Value < 0)
                throw new ArgumentException("đơn hàng tối thiểu phải là số không âm");

            if (expiryDate.HasValue && expiryDate.Value <= DateTime.UtcNow)
                throw new ArgumentException("Hạn sử dụng phải là thời gian trong tương lai");

            if (usageCount.HasValue && usageCount.Value < 0)
                throw new ArgumentException("Số lượng đã sử dụng phải là số không âm");

            if (maxUsage.HasValue && maxUsage.Value < 0)
                throw new ArgumentException("Số lượng tối đa phải là số không âm");

            if (usageCount.HasValue && maxUsage.HasValue && usageCount.Value > maxUsage.Value)
                throw new ArgumentException("Số lượng đã sử dụng không thể lớn hơn số lượng tối đa");

            return new Voucher
            {
                Code = trimmedCode,
                DiscountAmount = discountAmount,
                MaxDiscountAmount = maxDiscountAmount,
                MinOrderAmount = minOrderAmount,
                ExpiryDate = expiryDate,
                UsageCount = usageCount,
                MaxUsage = maxUsage
            };
        }
    }
}
