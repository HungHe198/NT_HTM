using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admin");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Position).HasMaxLength(100);
            builder.HasOne(x => x.User).WithOne(u => u.Admin).HasForeignKey<Admin>(x => x.UserId);
        }
    }
}
