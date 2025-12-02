using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Address).HasMaxLength(300);
            builder.Property(x => x.Gender).HasMaxLength(20);
            builder.HasOne(x => x.User).WithOne(u => u.Customer).HasForeignKey<Customer>(x => x.UserId);
        }
    }
}
