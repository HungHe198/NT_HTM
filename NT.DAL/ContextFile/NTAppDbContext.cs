using Microsoft.EntityFrameworkCore;
using NT.SHARED.Configurations;
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
            // Only configure if not already configured (by dependency injection)
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LEVANHUNG\\LEVANHUNG;Initial Catalog=NT_HTM;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Order: lookup & core security
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());

            // Users
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());

            // Catalog roots
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());

            // Technical attribute tables
            modelBuilder.ApplyConfiguration(new LengthConfiguration());
            modelBuilder.ApplyConfiguration(new SurfaceFinishConfiguration());
            modelBuilder.ApplyConfiguration(new HardnessConfiguration());
            modelBuilder.ApplyConfiguration(new ElasticityConfiguration());
            modelBuilder.ApplyConfiguration(new OriginCountryConfiguration());
            modelBuilder.ApplyConfiguration(new ColorConfiguration());

            // Product detail & media
            modelBuilder.ApplyConfiguration(new ProductDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());

            // Cart
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartDetailConfiguration());

            // Payment & discount
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherConfiguration());

            // Orders
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());

            // Data seeding
            NT.DAL.Services.DataSeeder.Apply(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        // DbSets
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Length> Lengths => Set<Length>();
        public DbSet<SurfaceFinish> SurfaceFinishes => Set<SurfaceFinish>();
        public DbSet<Hardness> Hardnesses => Set<Hardness>();
        public DbSet<Elasticity> Elasticities => Set<Elasticity>();
        public DbSet<OriginCountry> OriginCountries => Set<OriginCountry>();
        public DbSet<Color> Colors => Set<Color>();
        public DbSet<ProductDetail> ProductDetails => Set<ProductDetail>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartDetail> CartDetails => Set<CartDetail>();
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<Voucher> Vouchers => Set<Voucher>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    }
}
