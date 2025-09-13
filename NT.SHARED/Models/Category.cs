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
        [Required, MaxLength(100)]
        public string Name { get; private set; } = null!; // Tên danh mục (VD: Cần câu biển)
        [MaxLength(200)]
        public string? Description { get; private set; } // Mô tả danh mục

        // Private constructor for EF
        private Category() { }

        // Public static factory
        public static Category Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên danh mục không được để trống", nameof(name));
            return new Category
            {
                Name = name,
                Description = description
            };
        }

        // Navigation
        public ICollection<CategoryProduct> CategoryProducts { get; private set; } = new List<CategoryProduct>();
    }
}