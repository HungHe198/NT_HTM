using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.ToTable("ProductDetail");
            builder.HasKey(x => x.Id);

            // Decimal properties
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CostPrice).HasColumnType("decimal(18,2)");
           

            // Relationships
            builder.HasOne(x => x.Product)
                   .WithMany(p => p.ProductDetails)
                   .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.Length)
                   .WithMany(l => l.ProductDetails)
                   .HasForeignKey(x => x.LengthId);

            builder.HasOne(x => x.SurfaceFinish)
                   .WithMany(s => s.ProductDetails)
                   .HasForeignKey(x => x.SurfaceFinishId);

            builder.HasOne(x => x.Hardness)
                   .WithMany(h => h.ProductDetails)
                   .HasForeignKey(x => x.HardnessId);

            builder.HasOne(x => x.Elasticity)
                   .WithMany(e => e.ProductDetails)
                   .HasForeignKey(x => x.ElasticityId);

            builder.HasOne(x => x.OriginCountry)
                   .WithMany(o => o.ProductDetails)
                   .HasForeignKey(x => x.OriginCountryId);

            builder.HasOne(x => x.Color)
                   .WithMany(c => c.ProductDetails)
                   .HasForeignKey(x => x.ColorId);
        }
    }
}