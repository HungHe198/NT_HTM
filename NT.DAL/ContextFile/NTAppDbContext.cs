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
            optionsBuilder.UseSqlServer("Data Source=LEVANHUNG\\LEVANHUNG;Initial Catalog=DATN_NT;Integrated Security=True;Trust Server Certificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CouponConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDetailColorConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDetailHardnessConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDetailLengthConfiguration());
            modelBuilder.ApplyConfiguration(new RodColorConfiguration());
            modelBuilder.ApplyConfiguration(new RodHardnessConfiguration());
            modelBuilder.ApplyConfiguration(new RodLengthConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartDetailConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        //private static void SeedData(ModelBuilder modelBuilder)
        //{

        //}

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public DbSet<RodColor> RodColors { get; set; } = null!;
        public DbSet<ProductDetailColor> ProductDetailColors { get; set; } = null!;
        public DbSet<RodHardness> RodHardnesses { get; set; } = null!;
        public DbSet<ProductDetailHardness> ProductDetailHardnesses { get; set; } = null!;
        public DbSet<RodLength> RodLengths { get; set; } = null!;
        public DbSet<ProductDetailLength> ProductDetailLengths { get; set; } = null!;
        public DbSet<Coupon> Coupons { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartDetail> CartDetails { get; set; } = null!;
    }
}
