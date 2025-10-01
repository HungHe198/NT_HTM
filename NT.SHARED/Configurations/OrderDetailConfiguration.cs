using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.LengthAtOrder).HasColumnType("decimal(18,2)");
            builder.Property(x => x.NameAtOrder).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ColorAtOrder).IsRequired().HasMaxLength(50);
            builder.Property(x => x.HardnessAtOrder).IsRequired().HasMaxLength(50);

            builder.HasOne(x => x.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.ProductDetail)
                   .WithMany(p => p.OrderDetails)
                   .HasForeignKey(x => x.ProductDetailId);

        }
    }
}