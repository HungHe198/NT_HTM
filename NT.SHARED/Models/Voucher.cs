using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Voucher
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(50), Display(Name = "Mã voucher")]
        public string Code { get; set; } = null!;
        [Display(Name = "S? ti?n gi?m")]
        public decimal? DiscountAmount { get; set; }
        [Display(Name = "Gi?m t?i ?a")]
        public decimal? MaxDiscountAmount { get; set; }
        [Display(Name = "??n hàng t?i thi?u")]
        public decimal? MinOrderAmount { get; set; }
        [Display(Name = "H?n s? d?ng")]
        public DateTime? ExpiryDate { get; set; }
        [Display(Name = "S? l?n ?ã s? d?ng")]
        public int? UsageCount { get; set; }
        [Display(Name = "S? l?n t?i ?a")]
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
                throw new ArgumentException("Vui lòng nh?p mã voucher");

            var trimmedCode = code.Trim();
            if (trimmedCode.Length > 50)
                throw new ArgumentException("Mã voucher không ???c dài quá 50 ký t?");

            if (discountAmount.HasValue && discountAmount.Value < 0)
                throw new ArgumentException("S? ti?n gi?m ph?i là s? không âm");

            if (maxDiscountAmount.HasValue && maxDiscountAmount.Value < 0)
                throw new ArgumentException("Gi?m t?i ?a ph?i là s? không âm");

            if (discountAmount.HasValue && maxDiscountAmount.HasValue && maxDiscountAmount.Value < discountAmount.Value)
                throw new ArgumentException("Gi?m t?i ?a ph?i l?n h?n ho?c b?ng s? ti?n gi?m");

            if (minOrderAmount.HasValue && minOrderAmount.Value < 0)
                throw new ArgumentException("??n hàng t?i thi?u ph?i là s? không âm");

            if (expiryDate.HasValue && expiryDate.Value <= DateTime.UtcNow)
                throw new ArgumentException("H?n s? d?ng ph?i là th?i ?i?m trong t??ng lai");

            if (usageCount.HasValue && usageCount.Value < 0)
                throw new ArgumentException("S? l?n ?ã s? d?ng ph?i là s? không âm");

            if (maxUsage.HasValue && maxUsage.Value < 0)
                throw new ArgumentException("S? l?n t?i ?a ph?i là s? không âm");

            if (usageCount.HasValue && maxUsage.HasValue && usageCount.Value > maxUsage.Value)
                throw new ArgumentException("S? l?n ?ã s? d?ng không th? l?n h?n s? l?n t?i ?a");

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
