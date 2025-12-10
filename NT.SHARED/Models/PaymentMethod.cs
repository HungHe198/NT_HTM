using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class PaymentMethod
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(100), Display(Name = "Tên ph??ng th?c thanh toán")]
        public string Name { get; set; } = null!;
        [MaxLength(250), Display(Name = "Mô t?")]
        public string? Description { get; set; }

        public PaymentMethod() { }
        public static PaymentMethod Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vui lòng nh?p tên ph??ng th?c thanh toán");
            return new PaymentMethod { Name = name.Trim(), Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim() };
        }

        public ICollection<Order>? Orders { get; set; } = new List<Order>();
    }
}
