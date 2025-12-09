using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NT.SHARED.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Tên khách hàng")]
        public Guid CustomerId { get; set; }
        [Display(Name = "M? gi?m giá")]
        public Guid? VoucherId { get; set; }
        [Display(Name = "Phýõng th?c thanh toán")]
        public Guid PaymentMethodId { get; set; }
        [Display(Name = "Nhân viên t?o hóa ðõn")]
        public Guid? CreatedByUserId { get; set; }
        [Display(Name = "Th?i gian ð?t hàng")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        [Display(Name = "T?ng ti?n")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Ti?n ðý?c gi?m")]
        public decimal? DiscountAmount { get; set; }
        [Display(Name = "Ti?n khách ph?i tr?")]
        public decimal FinalAmount { get; set; }
        [Display(Name = "Tr?ng thái")]
        public string? Status { get; set; }
        [Display(Name = "S? ði?n tho?i")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Ð?a ch? giao hàng")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Ghi chú cho ðõn hàng")]
        public string? Note { get; set; }

        public Order() { }
        public static Order Create(Guid customerId, Guid paymentMethodId, decimal totalAmount, decimal finalAmount, string phoneNumber, string shippingAddress)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("Vui l?ng ðãng nh?p l?i ð? th?c hi?n ch?c nãng này (104)!");
            if (paymentMethodId == Guid.Empty) throw new ArgumentException("Vui l?ng ch?n phýõng th?c thanh toán!");

            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Vui l?ng nh?p s? ði?n tho?i!");

            // Normalize: remove non-digit chars
            var digits = Regex.Replace(phoneNumber, "\\D", "");
            // If starts with country code 84 (e.g. +849xxxxxxxx) convert to leading 0
            if (digits.StartsWith("84") && digits.Length == 11)
            {
                digits = "0" + digits.Substring(2);
            }

            // Vietnamese mobile numbers (10 digits) typically start with 03, 05, 07, 08, 09
            var vnPattern = new Regex("^0(3|5|7|8|9)\\d{8}$");
            if (!vnPattern.IsMatch(digits))
            {
                throw new ArgumentException("S? ði?n tho?i không h?p l?. Vui l?ng nh?p s? ði?n tho?i di ð?ng Vi?t Nam (ví d?: 0901234567 ho?c +84901234567).");
            }

            return new Order
            {
                CustomerId = customerId,
                PaymentMethodId = paymentMethodId,
                TotalAmount = totalAmount,
                FinalAmount = finalAmount,
                PhoneNumber = digits,
                ShippingAddress = string.IsNullOrWhiteSpace(shippingAddress) ? string.Empty : shippingAddress.Trim()
            };
        }

        public Customer? Customer { get; set; } 
        public Voucher? Voucher { get; set; }
        public PaymentMethod? PaymentMethod { get; set; } 
        public ICollection<OrderDetail>? Details { get; set; } = new List<OrderDetail>();
    }
}
