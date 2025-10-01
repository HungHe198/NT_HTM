using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.HasKey(x => new { x.CartId, x.ProductDetailId });

            builder.Property(x => x.PriceAtAdd).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Cart)
                   .WithMany(c => c.CartDetails)
                   .HasForeignKey(x => x.CartId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProductDetail)
                   .WithMany(pd => pd.CartDetails)
                   .HasForeignKey(x => x.ProductDetailId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
