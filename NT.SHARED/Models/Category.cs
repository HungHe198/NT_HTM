using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Category
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(150)]
        public string Name { get; private set; } = null!;
        [MaxLength(250)]
        public string? Description { get; private set; }

        private Category() { }
        public static Category Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name required");
            return new Category { Name = name.Trim(), Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim() };
        }

        public ICollection<ProductCategory> ProductCategories { get; private set; } = new List<ProductCategory>();
    }
}
