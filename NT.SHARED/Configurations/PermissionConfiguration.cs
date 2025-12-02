using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permission");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.Property(x => x.Resource).HasMaxLength(100);
            builder.Property(x => x.Action).HasMaxLength(100);
            builder.Property(x => x.Method).HasMaxLength(10);
        }
    }
}
