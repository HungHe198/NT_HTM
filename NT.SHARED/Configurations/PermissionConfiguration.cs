using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(p => p.Resource)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(p => p.Action)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(p => p.Endpoint)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(p => p.HttpMethod)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(p => p.ViewPath)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(p => p.Description)
                .HasMaxLength(100);

            builder.HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId);
        }
    }
}
