using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NT.SHARED.Models;
using System;

namespace NT.SHARED.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);

            // Note: seeding with fixed GUIDs removed to avoid predictable IDs.
            // Runtime seeding is performed in Program.cs to create roles with generated GUIDs.
        }
    }
}
