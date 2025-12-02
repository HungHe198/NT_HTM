using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Fullname).IsRequired().HasMaxLength(150);
            builder.Property(x => x.PhoneNumber).HasMaxLength(20);
            builder.Property(x => x.Email).HasMaxLength(150);
            builder.Property(x => x.Status).HasMaxLength(50);
            builder.HasOne(x => x.Role).WithMany(r => r.Users).HasForeignKey(x => x.RoleId);
        }
    }
}
