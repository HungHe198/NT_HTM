using Microsoft.EntityFrameworkCore;
using NT.SHARED.Configurations;
using NT.SHARED.Configurations.NT.SHARED.Models.Configurations;
using NT.SHARED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.DAL.ContextFile
{
    public class NTAppDbContext : DbContext
    {
        public NTAppDbContext()
        {
        }
        public NTAppDbContext(DbContextOptions options) : base(options)
        {
        }

        

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=LEVANHUNG\\LEVANHUNG;Initial Catalog=DATN_NT;Integrated Security=True;Trust Server Certificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Áp dụng các cấu hình từ NT.SHARED.Configuration
            modelBuilder.ApplyConfiguration(new ActorTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDetailConfiguration());
            modelBuilder.ApplyConfiguration(new RodColorConfiguration());
            modelBuilder.ApplyConfiguration(new RodHardnessConfiguration());
            modelBuilder.ApplyConfiguration(new RodLengthConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryProductConfiguration());

            // Seed data (sử dụng factory methods)
            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        private static void SeedData(ModelBuilder modelBuilder)
        {
          
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ActorType> ActorTypes { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
    }
}
