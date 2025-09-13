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
    public class RodLengthConfiguration : IEntityTypeConfiguration<RodLength>
    {
        public void Configure(EntityTypeBuilder<RodLength> builder)
        {
            builder.HasKey(rl => rl.Id);
            builder.Property(rl => rl.Value)
                .IsRequired();
            builder.Property(rl => rl.Unit)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasMany(rl => rl.ProductDetails)
                .WithOne(pd => pd.RodLength)
                .HasForeignKey(pd => pd.RodLengthId);
        }
    }
}
