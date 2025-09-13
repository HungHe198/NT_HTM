using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => new { od.OrderId, od.ProductId, od.RodHardnessId, od.RodLengthId, od.RodColorId });

            builder.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            builder.HasOne(od => od.ProductDetail)
                .WithMany(pd => pd.OrderDetails)
                .HasForeignKey(od => new { od.ProductId, od.RodHardnessId, od.RodLengthId, od.RodColorId });

            builder.Property(od => od.Quantity)
                .IsRequired();
            builder.Property(od => od.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}