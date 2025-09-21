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
                builder.HasKey(x => x.Id);

                builder.HasOne(x => x.Customer)
                       .WithMany(c => c.Orders)
                       .HasForeignKey(x => x.CustomerId)
                       .OnDelete(DeleteBehavior.Cascade); // hợp lý: Xóa khách hàng thì đơn hàng mất luôn

                builder.HasOne(x => x.Coupon)
                       .WithMany(c => c.Orders)
                       .HasForeignKey(x => x.CouponId)
                       .OnDelete(DeleteBehavior.SetNull); // xóa coupon thì Order giữ lại nhưng CouponId = null

                builder.HasOne(x => x.CreatedByUser)
                       .WithMany(u => u.Orders)
                       .HasForeignKey(x => x.CreatedByUserId)
                       .OnDelete(DeleteBehavior.NoAction); // tránh multiple cascade
            }
        }
    }
}
