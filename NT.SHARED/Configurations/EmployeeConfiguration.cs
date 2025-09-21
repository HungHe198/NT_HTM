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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            // 1-1: Employee <-> User
            builder.HasOne(e => e.User)
                   .WithOne(u => u.Employee)
                   .HasForeignKey<Employee>(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
