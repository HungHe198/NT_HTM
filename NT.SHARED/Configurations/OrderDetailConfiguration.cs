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

            builder.HasOne(x => x.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.ProductDetail)
                   .WithMany(p => p.OrderDetails)
                   .HasForeignKey(x => x.ProductDetailId);

        }
    }
}