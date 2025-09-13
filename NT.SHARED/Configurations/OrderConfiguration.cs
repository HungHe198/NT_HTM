using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Configurations
{
    using global::NT.SHARED.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NT.SHARED.Models;

    namespace NT.SHARED.Models.Configurations
    {
        public class OrderConfiguration : IEntityTypeConfiguration<Order>
        {
            public void Configure(EntityTypeBuilder<Order> builder)
            {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.UserId)
                    .IsRequired();
                builder.Property(o => o.OrderDate)
                    .IsRequired();
                builder.Property(o => o.TotalAmount)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                builder.Property(o => o.Status)
                    .IsRequired()
                    .HasMaxLength(50);
                builder.Property(o => o.ShippingAddress)
                    .HasMaxLength(200);

                builder.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId);

                builder.HasMany(o => o.OrderDetails)
                    .WithOne(od => od.Order)
                    .HasForeignKey(od => od.OrderId);
            }
        }
    }
}
