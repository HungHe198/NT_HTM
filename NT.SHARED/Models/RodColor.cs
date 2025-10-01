using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class RodColor
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(50)]
        public string Name { get; private set; } = null!;

        private RodColor() { }

        public static RodColor Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên màu không được để trống");
            return new RodColor { Name = name.Trim() };
        }

        public ICollection<ProductDetailColor> ProductDetailColors { get; private set; } = new List<ProductDetailColor>();
    }

}