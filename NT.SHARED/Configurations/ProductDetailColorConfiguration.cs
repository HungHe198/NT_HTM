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
    public class ProductDetailColorConfiguration : IEntityTypeConfiguration<ProductDetailColor>
    {
        public void Configure(EntityTypeBuilder<ProductDetailColor> builder)
        {
            builder.HasKey(x => new { x.ProductDetailId, x.RodColorId });
        }
    }
}
