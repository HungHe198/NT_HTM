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
    public class ActorTypeConfiguration : IEntityTypeConfiguration<ActorType>
    {
        public void Configure(EntityTypeBuilder<ActorType> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(a => a.Users)
                .WithOne(u => u.ActorType)
                .HasForeignKey(u => u.ActorTypeId);

            builder.HasMany(a => a.Roles)
                .WithOne(r => r.ActorType)
                .HasForeignKey(r => r.ActorTypeId);
        }
    }
}
