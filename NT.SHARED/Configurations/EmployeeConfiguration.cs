using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;

namespace NT.SHARED.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Position).HasMaxLength(100);
            builder.HasOne(x => x.User).WithOne(u => u.Employee).HasForeignKey<Employee>(x => x.UserId);
            builder.HasOne<Employee>().WithMany().HasForeignKey(x => x.ManagerId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
