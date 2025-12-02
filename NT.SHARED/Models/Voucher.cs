using System;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Voucher
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Code { get; private set; } = null!;
        public decimal? DiscountAmount { get; private set; }
        public decimal? MaxDiscountAmount { get; private set; }
        public decimal? MinOrderAmount { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public int? UsageCount { get; private set; }
        public int? MaxUsage { get; private set; }

        private Voucher() { }
        public static Voucher Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code required");
            return new Voucher { Code = code.Trim() };
        }
    }
}
