using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Configurations
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(pd => new { pd.ProductId, pd.RodHardnessId, pd.RodLengthId, pd.RodColorId });

            builder.HasOne(pd => pd.Product)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(pd => pd.ProductId);

            builder.HasOne(pd => pd.RodHardness)
                .WithMany(rh => rh.ProductDetails)
                .HasForeignKey(pd => pd.RodHardnessId);

            builder.HasOne(pd => pd.RodLength)
                .WithMany(rl => rl.ProductDetails)
                .HasForeignKey(pd => pd.RodLengthId);

            builder.HasOne(pd => pd.RodColor)
                .WithMany(rc => rc.ProductDetails)
                .HasForeignKey(pd => pd.RodColorId);

            builder.Property(pd => pd.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(pd => pd.StockQuantity)
                .IsRequired();
            builder.Property(pd => pd.Material)
                .HasMaxLength(50);
        }
    }
}
