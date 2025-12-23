using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    /// <summary>
    /// Kênh bán hàng áp dụng cho phương thức thanh toán
    /// </summary>
    public enum SalesChannel
    {
        /// <summary>
        /// Áp dụng cho cả Online và Offline
        /// </summary>
        All = 0,
        /// <summary>
        /// Chỉ áp dụng cho bán Online (COD)
        /// </summary>
        Online = 1,
        /// <summary>
        /// Chỉ áp dụng cho bán Offline/POS (Tiền mặt, Chuyển khoản)
        /// </summary>
        Offline = 2
    }

    public class PaymentMethod
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(100), Display(Name = "Tên phương thức thanh toán")]
        public string Name { get; set; } = null!;
        [MaxLength(250), Display(Name = "Mô tả")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Kênh bán hàng áp dụng: 0 = Tất cả, 1 = Online, 2 = Offline/POS
        /// </summary>
        [Display(Name = "Kênh bán hàng")]
        public SalesChannel Channel { get; set; } = SalesChannel.All;

        public PaymentMethod() { }
        public static PaymentMethod Create(string name, string? description = null, SalesChannel channel = SalesChannel.All)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui lòng nhập tên phương thức thanh toán");
            return new PaymentMethod 
            { 
                Name = name.Trim(), 
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                Channel = channel
            };
        }

        /// <summary>
        /// Kiểm tra phương thức thanh toán có áp dụng cho Online không
        /// </summary>
        public bool IsAvailableForOnline() => Channel == SalesChannel.All || Channel == SalesChannel.Online;

        /// <summary>
        /// Kiểm tra phương thức thanh toán có áp dụng cho Offline/POS không
        /// </summary>
        public bool IsAvailableForOffline() => Channel == SalesChannel.All || Channel == SalesChannel.Offline;

        public ICollection<Order>? Orders { get; set; } = new List<Order>();
    }
}
