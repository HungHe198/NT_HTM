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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(r => r.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(r => r.ActorTypeId)
                .IsRequired();

            builder.HasOne(r => r.ActorType)
                .WithMany(a => a.Roles)
                .HasForeignKey(r => r.ActorTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Thay thế CASCADE bằng RESTRICT

            builder.HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Thay thế CASCADE bằng RESTRICT

            builder.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Thay thế CASCADE bằng RESTRICT
        }
    }
}
