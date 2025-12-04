    using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Voucher
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Code { get; set; } = null!;
        public decimal? DiscountAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? UsageCount { get; set; }
        public int? MaxUsage { get; set; }

        public Voucher() { }
        public static Voucher Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code required");
            return new Voucher { Code = code.Trim() };
        }
    }
}
