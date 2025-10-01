using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt);

            builder.HasOne(x => x.Customer)
                   .WithOne(c => c.Cart)
                   .HasForeignKey<Cart>(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
