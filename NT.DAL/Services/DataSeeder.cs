using Microsoft.EntityFrameworkCore;
using NT.SHARED.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace NT.DAL.Services
{
    public static class DataSeeder
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            // Seed Brands (cần câu đài)
            var brandDefault = Guid.NewGuid();
            var brands = new List<Brand>
            {
                new Brand { Id = brandDefault, Name = "NT Fishing", Website = "https://ntfishing.example" },
                new Brand { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Daiwa", Website = "https://www.daiwa.com" },
                new Brand { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Shimano", Website = "https://www.shimano.com" },
                new Brand { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "Okuma", Website = "https://www.okumafishing.com" },
                new Brand { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Trabucco", Website = "https://www.trabucco.it" }
            };
            modelBuilder.Entity<Brand>().HasData(brands);

            // Seed required lookups used by ProductDetail FK
            var colorSample = new Color { Id = Guid.Parse("66666666-6666-6666-6666-000000000001"), Name = "Đen", HexCode = "#000000" };
            modelBuilder.Entity<Color>().HasData(colorSample);
            var elasticitySample = new Elasticity { Id = Guid.Parse("88888888-8888-8888-8888-000000000001"), Name = "Đàn hồi tiêu chuẩn" };
            modelBuilder.Entity<Elasticity>().HasData(elasticitySample);
            var originCountrySample = new OriginCountry { Id = Guid.Parse("77777777-7777-7777-7777-000000000001"), Name = "Việt Nam" };
            modelBuilder.Entity<OriginCountry>().HasData(originCountrySample);
            var surfaceFinishSampleConf = new SurfaceFinish { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Name = "Sơn bóng" };
            modelBuilder.Entity<SurfaceFinish>().HasData(surfaceFinishSampleConf);

            // Seed Lengths
            var lengths = new List<Length>
            {
                new Length { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "2m7", Description = "Cần câu đài 2m7" },
                new Length { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "3m9", Description = "Cần câu đài 3m9" },
                new Length { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "4m5", Description = "Cần câu đài 4m5" },
                new Length { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "5m4", Description = "Cần câu đài 5m4" },
                new Length { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "6m3", Description = "Cần câu đài 6m3" },
                new Length { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "7m2", Description = "Cần câu đài 7m2" },
                new Length { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "8m1", Description = "Cần câu đài 8m1" },
                new Length { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "9m", Description = "Cần câu đài 9m" },
            };
            modelBuilder.Entity<Length>().HasData(lengths);

            // Seed Hardness from 3H to 12H
            var hardnesses = new List<Hardness>();
            for (int h = 3; h <= 12; h++)
            {
                hardnesses.Add(new Hardness
                {
                    Id = Guid.Parse($"00000000-0000-0000-0000-0000000000{h:D2}"),
                    Name = $"{h}H",
                    Description = $"Độ cứng {h}H cho cần câu đài"
                });
            }
            modelBuilder.Entity<Hardness>().HasData(hardnesses);

            // Seed 50 Products (basic fields)
            // Randomized default BrandId seeded above
            var products = new List<Product>();
            var brandIds = brands.Select(b => b.Id).ToArray();
            var randBrand = new Random(5678);
            for (int i = 1; i <= 50; i++)
            {
                var randomBrandId = brandIds[randBrand.Next(0, brandIds.Length)];
                products.Add(new Product
                {
                    Id = Guid.Parse($"00000000-0000-0000-0000-{i:D12}"),
                    BrandId = randomBrandId,
                    ProductCode = $"CD{i:000}",
                    Name = $"Cần câu đài mẫu {i}",
                    ShortDescription = "Cần câu đài chất lượng, phù hợp hỗ dịch vụ và tự nhiên",
                    Thumbnail = "/images/product-placeholder.png",
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow
                });
            }
            modelBuilder.Entity<Product>().HasData(products);

            // Seed ProductDetails: 5 for each Product
            // NOTE: Requires valid FK IDs for SurfaceFinish, Elasticity, OriginCountry, Color.
            // Here we use fixed sample IDs; ensure corresponding seeds exist.
            var surfaceFinishSampleId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var elasticitySampleId = Guid.Parse("88888888-8888-8888-8888-000000000001");
            var originCountrySampleId = Guid.Parse("77777777-7777-7777-7777-000000000001");
            var colorSampleId = Guid.Parse("66666666-6666-6666-6666-000000000001");

            var details = new List<ProductDetail>();
            var lengthIds = lengths.Select(l => l.Id).ToArray();
            var hardnessIds = hardnesses.Select(h => h.Id).ToArray();
            var rand = new Random(1234);
            int detailCounter = 1;
            foreach (var p in products)
            {
                for (int k = 0; k < 5; k++)
                {
                    var lengthId = lengthIds[(detailCounter + k) % lengthIds.Length];
                    var hardnessId = hardnessIds[(detailCounter + k) % hardnessIds.Length];
                    var price = 500_000m + (rand.Next(0, 50) * 10_000m); // 500k - 1m
                    var stock = rand.Next(5, 50);
                    details.Add(new ProductDetail
                    {
                        Id = Guid.Parse($"00000000-0000-0000-0000-{detailCounter:D12}"),
                        ProductId = p.Id,
                        LengthId = lengthId,
                        SurfaceFinishId = surfaceFinishSampleId,
                        HardnessId = hardnessId,
                        ElasticityId = elasticitySampleId,
                        OriginCountryId = originCountrySampleId,
                        ColorId = colorSampleId,
                        Price = price,
                        StockQuantity = stock,
                        IsActive = true
                    });
                    detailCounter++;
                }
            }
            modelBuilder.Entity<ProductDetail>().HasData(details);

            // Seed Roles
            var roleAdminId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var roleEmployeeId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0001");
            var roleCustomerId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0001");
            modelBuilder.Entity<Role>().HasData(new { Id = roleAdminId, Name = "Admin" });
            modelBuilder.Entity<Role>().HasData(new { Id = roleEmployeeId, Name = "Employee" });
            modelBuilder.Entity<Role>().HasData(new { Id = roleCustomerId, Name = "Customer" });

            // Seed Admin User
            var adminUserId = Guid.Parse("11111111-2222-3333-4444-555555555555");
            var adminUser = new User
            {
                Id = adminUserId,
                RoleId = roleAdminId,
                Username = "admin",
                PasswordHash = string.Empty,
                Fullname = "System Administrator",
                Email = "admin@example.com",
                Status = "Active"
            };
            // Hash default password 'admin123' so login works
            var hasher = new PasswordHasher<User>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin123");
            modelBuilder.Entity<User>().HasData(adminUser);

            // Seed Admin profile linked to user
            modelBuilder.Entity<Admin>().HasData(new
            {
                Id = Guid.Parse("22222222-3333-4444-5555-666666666666"),
                UserId = adminUserId,
                Position = "Administrator",
                Salary = (decimal?)null
            });
        }
    }
}
