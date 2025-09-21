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
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(20);

            // 1-1: Admin <-> User
            builder.HasOne(a => a.User)
                   .WithOne(u => u.Admin)
                   .HasForeignKey<Admin>(a => a.UserId)   // chỉ rõ FK ở phía dependent
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
