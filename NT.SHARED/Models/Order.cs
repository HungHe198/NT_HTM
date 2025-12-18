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
        [Display(Name = "mã giảm giá")]
        public Guid? VoucherId { get; set; }
        [Display(Name = "Phương thức thanh toán")]
        public Guid PaymentMethodId { get; set; }
        [Display(Name = "Nhân viên tạo hóa đơn")]
        public Guid? CreatedByUserId { get; set; }
        [Display(Name = "Nhân viên xác nhận đơn")]
        public Guid? ConfirmedByUserId { get; set; }
        [Display(Name = "Nhân viên bàn giao đơn")]
        public Guid? HandoverByUserId { get; set; }
        [Display(Name = "Thời gian đơn hàng")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Tiền được giảm")]
        public decimal? DiscountAmount { get; set; }
        [Display(Name = "Tiền khách phải trả")]
        public decimal FinalAmount { get; set; }
        [Display(Name = "Trạng thái")]
        public string? Status { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Ðịa chỉ giao hàng")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Ghi chú cho đơn hàng")]
        public string? Note { get; set; }

        public Order() { }
        public static Order Create(Guid customerId, Guid paymentMethodId, decimal totalAmount, decimal finalAmount, string phoneNumber, string shippingAddress)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("Vui lòng đăng nhập lại để thực hiện chứcc nãng này (104)!");
            if (paymentMethodId == Guid.Empty) throw new ArgumentException("Vui lòng chọn phương thức thanh toán!");

            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Vui lòng nhập số điện thoại!");

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
                throw new ArgumentException("Số điện thoại không hợp lí. Vui lòng nhập số điện thoại di động Việt Nam (ví dụ: 0901234567 ho?c +84901234567).");
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
