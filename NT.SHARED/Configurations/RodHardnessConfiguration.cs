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
    public class RodHardnessConfiguration : IEntityTypeConfiguration<RodHardness>
    {
        public void Configure(EntityTypeBuilder<RodHardness> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
