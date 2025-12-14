using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Voucher");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.Property(x => x.DiscountPercentage).HasColumnType("decimal(5,2)");
            builder.Property(x => x.MaxDiscountAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.MinOrderAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.UsageCount).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.MaxUsage);
        }
    }
}