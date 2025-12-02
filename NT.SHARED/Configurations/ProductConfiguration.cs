using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductCode).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ShortDescription).HasMaxLength(500);
            builder.Property(x => x.Thumbnail).HasMaxLength(300);
            builder.Property(x => x.Status).HasMaxLength(50);
            builder.Property(x => x.SeoTitle).HasMaxLength(200);
            builder.Property(x => x.SeoDescription).HasMaxLength(300);
            builder.HasOne(x => x.Brand).WithMany(b => b.Products).HasForeignKey(x => x.BrandId);
        }
    }
}
