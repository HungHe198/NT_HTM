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
    public class RodColorConfiguration : IEntityTypeConfiguration<RodColor>
    {
        public void Configure(EntityTypeBuilder<RodColor> builder)
        {
            builder.HasKey(rc => rc.Id);
            builder.Property(rc => rc.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(rc => rc.Description)
                .HasMaxLength(200);

            builder.HasMany(rc => rc.ProductDetails)
                .WithOne(pd => pd.RodColor)
                .HasForeignKey(pd => pd.RodColorId);
        }
    }
}
