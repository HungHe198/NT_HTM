using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Category
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(150)]
        public string Name { get; private set; } = null!;

        private Category() { }

        public static Category Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên danh mục không được để trống");
            return new Category { Name = name.Trim() };
        }

        public ICollection<ProductCategory> ProductCategories { get; private set; } = new List<ProductCategory>();
    }
}