using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategory");
            builder.HasKey(x => new { x.CategoryId, x.ProductId });
            builder.HasOne(x => x.Category).WithMany(c => c.ProductCategories).HasForeignKey(x => x.CategoryId);
            builder.HasOne(x => x.Product).WithMany(p => p.ProductCategories).HasForeignKey(x => x.ProductId);
        }
    }
}
