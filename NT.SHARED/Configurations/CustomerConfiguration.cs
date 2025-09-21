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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Address)
                .HasMaxLength(250);

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(20);

            // 1-1: Customer <-> User
            builder.HasOne(c => c.User)
                   .WithOne(u => u.Customer)
                   .HasForeignKey<Customer>(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
