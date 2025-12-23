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
            // Seed Brands (cần câu đài) - Based on SQL data
            var brands = new List<Brand>
            {
                new Brand { Id = Guid.Parse("ba521afa-602b-4912-bc0f-1c28a431d26d"), Name = "Haidiao", Website = "https://haidiao.example" },
                new Brand { Id = Guid.Parse("9ab22734-bae9-429e-bbf6-6baae321a646"), Name = "Daiwa", Website = "https://www.daiwa.com" },
                new Brand { Id = Guid.Parse("2bafb8bf-a661-4ef4-9499-dbd459655c80"), Name = "MC", Website = "https://mc.example" },
                new Brand { Id = Guid.Parse("abd56e6c-e30f-407a-8a17-e50f38de390c"), Name = "Gama", Website = "https://g.example" },
                new Brand { Id = Guid.Parse("5a482c17-5bfe-4dbf-98be-24b7f4f03a80"), Name = "Bing", Website = "https://b.example" },
                new Brand { Id = Guid.Parse("e11dfd95-2327-4187-acf3-01063d7f44f3"), Name = "Shimano", Website = "https://www.shimano.com" }
            };
            modelBuilder.Entity<Brand>().HasData(brands);

            // Seed required lookups used by ProductDetail FK
            var colors = new List<Color>
            {
                new Color { Id = Guid.Parse("66666666-6666-6666-6666-000000000001"), Name = "Đen", HexCode = "#000000" },
                new Color { Id = Guid.Parse("7a964316-12ed-4b83-bd75-924e93528f5e"), Name = "Xanh", HexCode = "#0000FF" },
                new Color { Id = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Name = "Đỏ", HexCode = "#FF0000" },
                new Color { Id = Guid.Parse("79927f97-f7f2-4ecd-bed3-d89a5f9ed414"), Name = "Vàng", HexCode = "#FFFF00" },
                new Color { Id = Guid.Parse("9b701fba-417e-4791-85ad-fd9df6cfe703"), Name = "Trắng", HexCode = "#FFFFFF" },
                new Color { Id = Guid.Parse("e250bc3f-d40a-4b82-823a-fc1ad1526156"), Name = "Xám", HexCode = "#808080" }
            };
            modelBuilder.Entity<Color>().HasData(colors);

            var elasticities = new List<Elasticity>
            {
                new Elasticity { Id = Guid.Parse("88888888-8888-8888-8888-000000000001"), Name = "Đàn hồi tiêu chuẩn" },
                new Elasticity { Id = Guid.Parse("c335cf47-08b0-4faa-94c0-2d531b76c874"), Name = "Đàn hồi cao" }
            };
            modelBuilder.Entity<Elasticity>().HasData(elasticities);

            var originCountries = new List<OriginCountry>
            {
                new OriginCountry { Id = Guid.Parse("77777777-7777-7777-7777-000000000001"), Name = "Việt Nam" },
                new OriginCountry { Id = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), Name = "Trung Quốc" },
                new OriginCountry { Id = Guid.Parse("8d455725-d3a3-47d2-a8ae-1887b2595f87"), Name = "Nhật Bản" }
            };
            modelBuilder.Entity<OriginCountry>().HasData(originCountries);

            var surfaceFinishes = new List<SurfaceFinish>
            {
                new SurfaceFinish { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Name = "Sơn bóng" },
                new SurfaceFinish { Id = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), Name = "Sơn mờ" }
            };
            modelBuilder.Entity<SurfaceFinish>().HasData(surfaceFinishes);

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
                new Length { Id = Guid.Parse("fb1b4d03-dbb5-440e-b16c-a60c8181b17a"), Name = "3m6", Description = "Cần câu đài 3m6" }
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

            // Seed Products from SQL data - Status = "1" means Active (ProductStatus.Active)
            var products = new List<Product>
            {
                new Product { Id = Guid.Parse("bd482f00-c2a4-4b79-9441-260891ed1744"), BrandId = Guid.Parse("ba521afa-602b-4912-bc0f-1c28a431d26d"), ProductCode = "HD02", Name = "Chiến Vũ Lí", Thumbnail = "/uploads/products/79044a1b-bfb7-4488-86f0-23a0cc443b57.webp", Status = "1" },
                new Product { Id = Guid.Parse("62342343-5e30-41f2-8e73-2c13f72fedc7"), BrandId = Guid.Parse("9ab22734-bae9-429e-bbf6-6baae321a646"), ProductCode = "DW02", Name = "Tầm Vương", Thumbnail = "/uploads/products/250d649f-91de-4380-b4cc-62a1b08d4ac6.webp", Status = "1" },
                new Product { Id = Guid.Parse("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), BrandId = Guid.Parse("2bafb8bf-a661-4ef4-9499-dbd459655c80"), ProductCode = "MC01", Name = "Phục Ma Bá Đạo", Thumbnail = "/uploads/products/9c1fc107-bf90-4f99-a323-34394a7cd234.webp", Status = "1" },
                new Product { Id = Guid.Parse("45109257-a076-425e-98ae-3c29b232ea13"), BrandId = Guid.Parse("ba521afa-602b-4912-bc0f-1c28a431d26d"), ProductCode = "HD03", Name = "Bất Phàm", Thumbnail = "/uploads/products/78845cd3-cedf-4e4a-98d5-1df2cabac9d9.webp", Status = "1" },
                new Product { Id = Guid.Parse("2c7e99bd-94cb-415f-806d-5ac175805818"), BrandId = Guid.Parse("abd56e6c-e30f-407a-8a17-e50f38de390c"), ProductCode = "G01", Name = "Phục Long Săn Hàng", Thumbnail = "/uploads/products/f21a5144-e337-4a4b-ba8f-b65d7786e26b.webp", Status = "1" },
                new Product { Id = Guid.Parse("4ae4f11b-3e49-468f-84b7-73d100890f04"), BrandId = Guid.Parse("9ab22734-bae9-429e-bbf6-6baae321a646"), ProductCode = "DW04", Name = "Vũ Thiết Sa", ShortDescription = "Hangding Vũ thiết", Thumbnail = "/uploads/products/f8d0cb97-7780-49e9-bbd7-7683dbfbabb8.jpg", Status = "1" },
                new Product { Id = Guid.Parse("a7e70ded-b43d-4439-b57a-90894fd91083"), BrandId = Guid.Parse("5a482c17-5bfe-4dbf-98be-24b7f4f03a80"), ProductCode = "B01", Name = "Long Tịnh", Thumbnail = "/uploads/products/d2263568-4bc7-4885-8b61-abe984608073.jpg", Status = "1" },
                new Product { Id = Guid.Parse("3103e99c-5ca0-4093-9687-afb58a02b101"), BrandId = Guid.Parse("ba521afa-602b-4912-bc0f-1c28a431d26d"), ProductCode = "HD01", Name = "Đại Phong", Thumbnail = "/uploads/products/7234a007-a8cd-4c14-b96b-d6e9f25f3414.webp", Status = "1" },
                new Product { Id = Guid.Parse("4fbcad35-dd5e-4e69-811e-b01167a7adda"), BrandId = Guid.Parse("2bafb8bf-a661-4ef4-9499-dbd459655c80"), ProductCode = "HD06", Name = "Đại Hải", ShortDescription = "cần câu đại hải", Thumbnail = "/uploads/products/848ded34-9bbe-4440-963c-5cf3c376bc61.jpg", Status = "1" },
                new Product { Id = Guid.Parse("3d4087a9-8e60-4adc-9f3a-d3c3e037923b"), BrandId = Guid.Parse("9ab22734-bae9-429e-bbf6-6baae321a646"), ProductCode = "KW01", Name = "Hoành Thiên Hạ Limited", Thumbnail = "/uploads/products/4f662526-8278-41e5-90fa-de03725eede5.webp", Status = "1" },
                new Product { Id = Guid.Parse("fc6f1a89-2072-4d30-8782-d61f1c66b425"), BrandId = Guid.Parse("e11dfd95-2327-4187-acf3-01063d7f44f3"), ProductCode = "SH01", Name = "Lôi Phong", Thumbnail = "/uploads/products/41426851-b611-4fe5-abba-b7dde6dae7d4.jpg", Status = "1" },
                new Product { Id = Guid.Parse("ad07e3ea-ed59-4c10-a634-dc30f4f17fb0"), BrandId = Guid.Parse("ba521afa-602b-4912-bc0f-1c28a431d26d"), ProductCode = "HD04", Name = "Chiến Plus", Thumbnail = "/uploads/products/7c5bceac-764c-4d00-b510-a733760f605b.webp", Status = "1" }
            };
            modelBuilder.Entity<Product>().HasData(products);

            // Seed ProductDetails from SQL data
            var productDetails = new List<ProductDetail>
            {
                new ProductDetail { Id = Guid.Parse("1d5b0c18-60f3-46a6-9df8-003211c32983"), ProductId = Guid.Parse("2c7e99bd-94cb-415f-806d-5ac175805818"), LengthId = Guid.Parse("55555555-5555-5555-5555-555555555555"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("c335cf47-08b0-4faa-94c0-2d531b76c874"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("66666666-6666-6666-6666-000000000001"), Sections = "6", CollapsedLength = "121cm", Weight = "172g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,6mm", TopDiameter = "15,3mm", ButtDiameter = "22mm", BalancePoint = "4000-5000g", BalanceLoadPoint = "9", Price = 2000000.00m, StockQuantity = 6, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("5d1e1667-89f9-4f08-83cc-04d732f78f4d"), ProductId = Guid.Parse("45109257-a076-425e-98ae-3c29b232ea13"), LengthId = Guid.Parse("33333333-3333-3333-3333-333333333333"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("66666666-6666-6666-6666-000000000001"), Sections = "5", CollapsedLength = "122cm", Weight = "172g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,6mm", TopDiameter = "20mm", ButtDiameter = "26mm", BalancePoint = "5000-6000g", BalanceLoadPoint = "8", Price = 900000.00m, StockQuantity = 3, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("3dbe0697-6b17-4854-a251-05db0b49a684"), ProductId = Guid.Parse("62342343-5e30-41f2-8e73-2c13f72fedc7"), LengthId = Guid.Parse("77777777-7777-7777-7777-777777777777"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("8d455725-d3a3-47d2-a8ae-1887b2595f87"), ColorId = Guid.Parse("7a964316-12ed-4b83-bd75-924e93528f5e"), Sections = "8", CollapsedLength = "121cm", Weight = "172g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,6mm", TopDiameter = "15,3mm", ButtDiameter = "28,9mm", BalancePoint = "7000g", BalanceLoadPoint = "8", Price = 9200000.00m, StockQuantity = 4, SoldQuantity = 3, LastImportDate = new DateTime(2025, 12, 15), IsActive = true },
                new ProductDetail { Id = Guid.Parse("e4dc208e-a1ba-4165-98f6-08d28324100a"), ProductId = Guid.Parse("3d4087a9-8e60-4adc-9f3a-d3c3e037923b"), LengthId = Guid.Parse("44444444-4444-4444-4444-444444444444"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("79927f97-f7f2-4ecd-bed3-d89a5f9ed414"), Sections = "5", CollapsedLength = "119g", Weight = "141g", TipWeight = "16g", ButtWeight = "26g", TipDiameter = "1,3mm", TopDiameter = "18,7mm", ButtDiameter = "23,8mm", BalancePoint = "4500", BalanceLoadPoint = "7", Price = 11560000.00m, StockQuantity = 25, SoldQuantity = 0, CostPrice = 8000000.00m, LastImportDate = new DateTime(2025, 12, 14), IsActive = true },
                new ProductDetail { Id = Guid.Parse("3dd3a835-efb9-4656-8128-1bccf2020e95"), ProductId = Guid.Parse("bd482f00-c2a4-4b79-9441-260891ed1744"), LengthId = Guid.Parse("55555555-5555-5555-5555-555555555555"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("c335cf47-08b0-4faa-94c0-2d531b76c874"), OriginCountryId = Guid.Parse("77777777-7777-7777-7777-000000000001"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "6", CollapsedLength = "122cm", Weight = "89g", TipWeight = "10g", ButtWeight = "20g", TipDiameter = "1,4mm", TopDiameter = "15,3mm", ButtDiameter = "26mm", BalancePoint = "4000-5000g", BalanceLoadPoint = "8", Price = 2100000.00m, StockQuantity = 7, SoldQuantity = 12, CostPrice = 1450000.00m, LastImportDate = new DateTime(2025, 12, 14), IsActive = true },
                new ProductDetail { Id = Guid.Parse("754ca79f-7c36-45f3-8c82-23e2978f1b6a"), ProductId = Guid.Parse("fc6f1a89-2072-4d30-8782-d61f1c66b425"), LengthId = Guid.Parse("55555555-5555-5555-5555-555555555555"), SurfaceFinishId = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("8d455725-d3a3-47d2-a8ae-1887b2595f87"), ColorId = Guid.Parse("79927f97-f7f2-4ecd-bed3-d89a5f9ed414"), Sections = "6", CollapsedLength = "118cm", Weight = "216g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,5mm", TopDiameter = "18,4mm", ButtDiameter = "23,4mm", BalancePoint = "6500", BalanceLoadPoint = "7", Price = 1500000.00m, StockQuantity = 25, SoldQuantity = 1, CostPrice = 900000.00m, LastImportDate = new DateTime(2025, 12, 14), IsActive = true },
                new ProductDetail { Id = Guid.Parse("014ffd65-5d70-4534-a11c-245361dafbe8"), ProductId = Guid.Parse("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), LengthId = Guid.Parse("77777777-7777-7777-7777-777777777777"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "8", CollapsedLength = "121cm", Weight = "528g", TipWeight = "24g", ButtWeight = "29g", TipDiameter = "1,3mm", TopDiameter = "27,9mm", ButtDiameter = "29,5mm", BalancePoint = "7000", BalanceLoadPoint = "8", Price = 2300000.00m, StockQuantity = 43, SoldQuantity = 2, CostPrice = 1100000.00m, LastImportDate = new DateTime(2025, 12, 12), IsActive = true },
                new ProductDetail { Id = Guid.Parse("606d0e6d-5f97-44c8-9223-2466d0f70b04"), ProductId = Guid.Parse("ad07e3ea-ed59-4c10-a634-dc30f4f17fb0"), LengthId = Guid.Parse("88888888-8888-8888-8888-888888888888"), SurfaceFinishId = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("c335cf47-08b0-4faa-94c0-2d531b76c874"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("7a964316-12ed-4b83-bd75-924e93528f5e"), Sections = "9", CollapsedLength = "121cm", Weight = "514g", TipWeight = "15g", ButtWeight = "30g", TipDiameter = "1,6mm", TopDiameter = "27,7mm", ButtDiameter = "28,9mm", BalancePoint = "5000-6000g", BalanceLoadPoint = "8", Price = 2400000.00m, StockQuantity = 70, SoldQuantity = 14, CostPrice = 1700000.00m, LastImportDate = new DateTime(2025, 12, 13), IsActive = true },
                new ProductDetail { Id = Guid.Parse("0677c96a-7474-4936-b8d6-24bd1c1ac08b"), ProductId = Guid.Parse("62342343-5e30-41f2-8e73-2c13f72fedc7"), LengthId = Guid.Parse("66666666-6666-6666-6666-666666666666"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "7", CollapsedLength = "120cm", Weight = "339g", TipWeight = "13g", ButtWeight = "29g", TipDiameter = "1,4mm", TopDiameter = "24,3mm", ButtDiameter = "27,2mm", BalancePoint = "6500", BalanceLoadPoint = "9", Price = 7180000.00m, StockQuantity = 37, SoldQuantity = 0, CostPrice = 5900000.00m, LastImportDate = new DateTime(2025, 12, 14), IsActive = true },
                new ProductDetail { Id = Guid.Parse("487f82fc-6678-4bf1-a3d8-25d2cada8633"), ProductId = Guid.Parse("a7e70ded-b43d-4439-b57a-90894fd91083"), LengthId = Guid.Parse("33333333-3333-3333-3333-333333333333"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000010"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("7a964316-12ed-4b83-bd75-924e93528f5e"), Sections = "5", CollapsedLength = "104cm", Weight = "121,6g", TipWeight = "8g", ButtWeight = "15g", TipDiameter = "1,1mm", TopDiameter = "10mm", ButtDiameter = "13,2mm", BalancePoint = "5000", BalanceLoadPoint = "7", Price = 7200000.00m, StockQuantity = 17, SoldQuantity = 2, CostPrice = 5600000.00m, LastImportDate = new DateTime(2025, 12, 12), IsActive = true },
                new ProductDetail { Id = Guid.Parse("268d8602-8ffb-4f08-af04-309d9ee7a652"), ProductId = Guid.Parse("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), LengthId = Guid.Parse("66666666-6666-6666-6666-666666666666"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "7", CollapsedLength = "121cm", Weight = "172g", TipWeight = "12g", ButtWeight = "26g", TipDiameter = "1,6mm", BalancePoint = "5000-6000g", BalanceLoadPoint = "8", Price = 2120000.00m, StockQuantity = 21, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("46a67c25-d91e-4cf6-8c24-3434dbd06a34"), ProductId = Guid.Parse("4ae4f11b-3e49-468f-84b7-73d100890f04"), LengthId = Guid.Parse("55555555-5555-5555-5555-555555555555"), SurfaceFinishId = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000011"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("77777777-7777-7777-7777-000000000001"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "23", CollapsedLength = "16", Weight = "35", TipWeight = "1", ButtWeight = "22", TipDiameter = "25", TopDiameter = "34", ButtDiameter = "32", BalancePoint = "11", BalanceLoadPoint = "22", RecommendedLine = "30m", RecommendedFishWeight = "5-7kg", Price = 1200000.00m, StockQuantity = 38, SoldQuantity = 35, CostPrice = 500000.00m, LastImportDate = new DateTime(2025, 12, 19), IsActive = true },
                new ProductDetail { Id = Guid.Parse("d7dc2fc2-cf47-453a-ba5c-4f21e33fe112"), ProductId = Guid.Parse("2c7e99bd-94cb-415f-806d-5ac175805818"), LengthId = Guid.Parse("66666666-6666-6666-6666-666666666666"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "7", CollapsedLength = "121cm", Weight = "172g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,6mm", TopDiameter = "20mm", ButtDiameter = "26mm", BalancePoint = "5000-6000g", BalanceLoadPoint = "9", Price = 2300000.00m, StockQuantity = 9, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("49219172-62e3-4e6d-bbb8-57b7466a59f4"), ProductId = Guid.Parse("4fbcad35-dd5e-4e69-811e-b01167a7adda"), LengthId = Guid.Parse("44444444-4444-4444-4444-444444444444"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000003"), ElasticityId = Guid.Parse("c335cf47-08b0-4faa-94c0-2d531b76c874"), OriginCountryId = Guid.Parse("8d455725-d3a3-47d2-a8ae-1887b2595f87"), ColorId = Guid.Parse("79927f97-f7f2-4ecd-bed3-d89a5f9ed414"), Sections = "16", CollapsedLength = "14", Weight = "53", TipWeight = "32", ButtWeight = "26", TipDiameter = "31", TopDiameter = "56", ButtDiameter = "32", BalancePoint = "11", BalanceLoadPoint = "14", RecommendedLine = "30m", RecommendedFishWeight = "15-17kg", Price = 4200000.00m, StockQuantity = 24, SoldQuantity = 23, CostPrice = 2800000.00m, LastImportDate = new DateTime(2025, 12, 19), IsActive = true },
                new ProductDetail { Id = Guid.Parse("72ddf35a-f7c9-420e-9dd2-650c6082779d"), ProductId = Guid.Parse("3103e99c-5ca0-4093-9687-afb58a02b101"), LengthId = Guid.Parse("44444444-4444-4444-4444-444444444444"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("9b701fba-417e-4791-85ad-fd9df6cfe703"), Sections = "5", CollapsedLength = "122cm", Weight = "172g", TipWeight = "12g", ButtWeight = "20g", TipDiameter = "1,4mm", TopDiameter = "20mm", ButtDiameter = "26mm", BalancePoint = "2600-3100g", BalanceLoadPoint = "7", Price = 1000000.00m, StockQuantity = 27, SoldQuantity = 25, CostPrice = 700000.00m, LastImportDate = new DateTime(2025, 12, 14), IsActive = true },
                new ProductDetail { Id = Guid.Parse("81c268b2-a541-48b0-8e98-73b92512b1ad"), ProductId = Guid.Parse("bd482f00-c2a4-4b79-9441-260891ed1744"), LengthId = Guid.Parse("44444444-4444-4444-4444-444444444444"), SurfaceFinishId = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("c335cf47-08b0-4faa-94c0-2d531b76c874"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("7a964316-12ed-4b83-bd75-924e93528f5e"), Sections = "5", CollapsedLength = "121cm", Weight = "165,4g", TipWeight = "15g", ButtWeight = "34g", TipDiameter = "1,6mm", TopDiameter = "17", ButtDiameter = "23,9", BalancePoint = "4000-5000g", BalanceLoadPoint = "7", Price = 1700000.00m, StockQuantity = 15, SoldQuantity = 10, CostPrice = 1100000.00m, LastImportDate = new DateTime(2025, 12, 14), IsActive = true },
                new ProductDetail { Id = Guid.Parse("b111948c-c3e3-41ce-93c5-831b90897fd7"), ProductId = Guid.Parse("45109257-a076-425e-98ae-3c29b232ea13"), LengthId = Guid.Parse("44444444-4444-4444-4444-444444444444"), SurfaceFinishId = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("77777777-7777-7777-7777-000000000001"), ColorId = Guid.Parse("66666666-6666-6666-6666-000000000001"), Sections = "5", CollapsedLength = "121cm", Weight = "172g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,6mm", TopDiameter = "20mm", ButtDiameter = "26mm", BalancePoint = "5000-6000g", Price = 1340000.00m, StockQuantity = 23, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("ca1871c6-d19d-4712-b04a-92e3b9db7fe6"), ProductId = Guid.Parse("45109257-a076-425e-98ae-3c29b232ea13"), LengthId = Guid.Parse("55555555-5555-5555-5555-555555555555"), SurfaceFinishId = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("7a964316-12ed-4b83-bd75-924e93528f5e"), Sections = "6", Weight = "121cm", TipWeight = "18g", ButtWeight = "30g", TipDiameter = "1,7mm", TopDiameter = "21,8mm", ButtDiameter = "23,8mm", BalancePoint = "9000-11000g", BalanceLoadPoint = "9", Price = 1400000.00m, StockQuantity = 6, SoldQuantity = 15, CostPrice = 1000000.00m, LastImportDate = new DateTime(2025, 12, 14), IsActive = true },
                new ProductDetail { Id = Guid.Parse("2b77eed7-80c0-42e3-9bc5-94677c01dc99"), ProductId = Guid.Parse("bd482f00-c2a4-4b79-9441-260891ed1744"), LengthId = Guid.Parse("77777777-7777-7777-7777-777777777777"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "8", CollapsedLength = "122cm", Weight = "411g", TipWeight = "12g", ButtWeight = "30g", TipDiameter = "1,6mm", TopDiameter = "20mm", ButtDiameter = "26mm", BalancePoint = "5000-6000g", BalanceLoadPoint = "8", Price = 2890000.00m, StockQuantity = 6, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 13), IsActive = true },
                new ProductDetail { Id = Guid.Parse("049e2071-4402-47e0-b223-a0b224005ce6"), ProductId = Guid.Parse("4ae4f11b-3e49-468f-84b7-73d100890f04"), LengthId = Guid.Parse("66666666-6666-6666-6666-666666666666"), SurfaceFinishId = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000007"), ElasticityId = Guid.Parse("c335cf47-08b0-4faa-94c0-2d531b76c874"), OriginCountryId = Guid.Parse("77777777-7777-7777-7777-000000000001"), ColorId = Guid.Parse("e250bc3f-d40a-4b82-823a-fc1ad1526156"), Sections = "13", CollapsedLength = "18", Weight = "32", TipWeight = "2", ButtWeight = "12", TipDiameter = "25", TopDiameter = "44", ButtDiameter = "26", BalancePoint = "15", BalanceLoadPoint = "14", RecommendedLine = "30m", Price = 5000000.00m, StockQuantity = 27, SoldQuantity = 22, CostPrice = 3200000.00m, LastImportDate = new DateTime(2025, 12, 19), IsActive = true },
                new ProductDetail { Id = Guid.Parse("8f1f78bd-aac3-4ffe-9b67-a5b0546291f9"), ProductId = Guid.Parse("2c7e99bd-94cb-415f-806d-5ac175805818"), LengthId = Guid.Parse("55555555-5555-5555-5555-555555555555"), SurfaceFinishId = Guid.Parse("94ce350e-4bf0-499d-9b80-8c7c0a9510d0"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "6", CollapsedLength = "118cm", Weight = "286g", TipWeight = "18g", ButtWeight = "24g", TipDiameter = "1,28mm", TopDiameter = "19,3mm", ButtDiameter = "22,8mm", BalancePoint = "8000", BalanceLoadPoint = "8", Price = 1750000.00m, StockQuantity = 43, SoldQuantity = 0, CostPrice = 1268000.00m, LastImportDate = new DateTime(2025, 12, 12), IsActive = true },
                new ProductDetail { Id = Guid.Parse("6e3d5c21-c4c2-4a99-896d-a7cf79088041"), ProductId = Guid.Parse("bd482f00-c2a4-4b79-9441-260891ed1744"), LengthId = Guid.Parse("66666666-6666-6666-6666-666666666666"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("7a964316-12ed-4b83-bd75-924e93528f5e"), Sections = "7", CollapsedLength = "121cm", Weight = "310g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,6mm", TopDiameter = "20mm", ButtDiameter = "26mm", BalancePoint = "5000-6000g", BalanceLoadPoint = "8", Price = 2600000.00m, StockQuantity = 10, SoldQuantity = 1, CostPrice = 1600000.00m, LastImportDate = new DateTime(2025, 12, 13), IsActive = true },
                new ProductDetail { Id = Guid.Parse("06efd08e-338d-41aa-b6cb-ad5d63a8f412"), ProductId = Guid.Parse("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), LengthId = Guid.Parse("33333333-3333-3333-3333-333333333333"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "5", CollapsedLength = "122cm", Weight = "172g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,6mm", TopDiameter = "20mm", ButtDiameter = "23,8mm", BalancePoint = "4000-5000g", BalanceLoadPoint = "7", Price = 1450000.00m, StockQuantity = 11, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("e44202d7-7a1b-49ce-9573-aff799783500"), ProductId = Guid.Parse("62342343-5e30-41f2-8e73-2c13f72fedc7"), LengthId = Guid.Parse("88888888-8888-8888-8888-888888888888"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("66666666-6666-6666-6666-000000000001"), Sections = "9", CollapsedLength = "118cm", Weight = "534g", TipWeight = "18g", ButtWeight = "34g", TipDiameter = "1,6mm", TopDiameter = "26mm", ButtDiameter = "31mm", BalancePoint = "7800g", Price = 11160000.00m, StockQuantity = 3, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("26d96697-c8ca-4cb9-8314-be1d32386d74"), ProductId = Guid.Parse("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), LengthId = Guid.Parse("44444444-4444-4444-4444-444444444444"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "5", CollapsedLength = "121cm", Weight = "172g", ButtWeight = "18g", TopDiameter = "20mm", ButtDiameter = "26mm", BalancePoint = "5000-6000g", BalanceLoadPoint = "7", Price = 1790000.00m, StockQuantity = 13, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("55f5a1e5-4168-4170-a1a0-cff392249ca2"), ProductId = Guid.Parse("3103e99c-5ca0-4093-9687-afb58a02b101"), LengthId = Guid.Parse("fb1b4d03-dbb5-440e-b16c-a60c8181b17a"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000005"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("9b701fba-417e-4791-85ad-fd9df6cfe703"), Sections = "4", CollapsedLength = "108g", Weight = "89g", TipWeight = "10g", ButtWeight = "18g", TipDiameter = "1,5mm", TopDiameter = "15,3mm", ButtDiameter = "22mm", BalancePoint = "2500-3100g", BalanceLoadPoint = "7", Price = 600000.00m, StockQuantity = 15, SoldQuantity = 0, CostPrice = 300000.00m, LastImportDate = new DateTime(2025, 12, 14), IsActive = true },
                new ProductDetail { Id = Guid.Parse("d0025a02-fbd3-41b4-b444-d84c9e902e08"), ProductId = Guid.Parse("d4726e64-22ab-4936-8338-3b4b5a54e4dd"), LengthId = Guid.Parse("55555555-5555-5555-5555-555555555555"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000006"), ElasticityId = Guid.Parse("88888888-8888-8888-8888-000000000001"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("a0c2ae82-56fb-4af1-8a42-2678ad491853"), Sections = "6", CollapsedLength = "121cm", Weight = "172g", TipWeight = "15g", TipDiameter = "1,7mm", TopDiameter = "15,3mm", ButtDiameter = "26mm", BalancePoint = "5000-6000g", Price = 1800000.00m, StockQuantity = 45, SoldQuantity = 0, LastImportDate = new DateTime(2025, 12, 17), IsActive = true },
                new ProductDetail { Id = Guid.Parse("333c99b1-b645-4b45-8588-edd50e706c1e"), ProductId = Guid.Parse("ad07e3ea-ed59-4c10-a634-dc30f4f17fb0"), LengthId = Guid.Parse("77777777-7777-7777-7777-777777777777"), SurfaceFinishId = Guid.Parse("99999999-9999-9999-9999-999999999999"), HardnessId = Guid.Parse("00000000-0000-0000-0000-000000000010"), ElasticityId = Guid.Parse("c335cf47-08b0-4faa-94c0-2d531b76c874"), OriginCountryId = Guid.Parse("66c9552e-8714-4e99-8452-152632333abc"), ColorId = Guid.Parse("9b701fba-417e-4791-85ad-fd9df6cfe703"), Sections = "23", CollapsedLength = "16", Weight = "35", TipWeight = "6", ButtWeight = "22", TipDiameter = "25", TopDiameter = "44", ButtDiameter = "26", BalancePoint = "15", Price = 4500000.00m, StockQuantity = 26, SoldQuantity = 29, CostPrice = 3200000.00m, LastImportDate = new DateTime(2025, 12, 19), IsActive = true }
            };
            modelBuilder.Entity<ProductDetail>().HasData(productDetails);

            // Seed ProductImages from SQL data
            var productImages = new List<ProductImage>
            {
                new ProductImage { Id = Guid.Parse("c1cfc4d0-91f5-4b26-b00a-02afe7bc35ab"), ProductDetailId = Guid.Parse("049e2071-4402-47e0-b223-a0b224005ce6"), ImageUrl = "/uploads/product-details/049e2071-4402-47e0-b223-a0b224005ce6/92b2b03f-b1cc-4072-92c0-9ef39daa4848.jpg" },
                new ProductImage { Id = Guid.Parse("86544e67-5f25-4ac7-a009-1aea255cd595"), ProductDetailId = Guid.Parse("ca1871c6-d19d-4712-b04a-92e3b9db7fe6"), ImageUrl = "/uploads/product-details/ca1871c6-d19d-4712-b04a-92e3b9db7fe6/e27ac9e8-d238-4f9b-af60-d5f3abaf5e00.webp" },
                new ProductImage { Id = Guid.Parse("a4ccfc2a-632d-4bc7-935c-1cb2a76da18b"), ProductDetailId = Guid.Parse("333c99b1-b645-4b45-8588-edd50e706c1e"), ImageUrl = "/uploads/product-details/333c99b1-b645-4b45-8588-edd50e706c1e/a8f3f183-c89b-42e1-b22a-289734ab4a3a.jpg" },
                new ProductImage { Id = Guid.Parse("7d968c7e-afda-436d-9fdf-1dca5be0bd1b"), ProductDetailId = Guid.Parse("606d0e6d-5f97-44c8-9223-2466d0f70b04"), ImageUrl = "/uploads/product-details/606d0e6d-5f97-44c8-9223-2466d0f70b04/76014e04-8fde-46af-9e9c-0b5375c2484f.webp" },
                new ProductImage { Id = Guid.Parse("af8334b8-ce61-4abc-bc7b-2ae6de336e54"), ProductDetailId = Guid.Parse("5d1e1667-89f9-4f08-83cc-04d732f78f4d"), ImageUrl = "/uploads/product-details/5d1e1667-89f9-4f08-83cc-04d732f78f4d/69b7f261-36a2-4a15-ba4c-cc6707701fd3.webp" },
                new ProductImage { Id = Guid.Parse("166f611f-5a3d-4578-b298-313beb7527f1"), ProductDetailId = Guid.Parse("487f82fc-6678-4bf1-a3d8-25d2cada8633"), ImageUrl = "/uploads/product-details/487f82fc-6678-4bf1-a3d8-25d2cada8633/c93be9d3-62a4-436a-8fe0-e815dbbba3a6.jpg" },
                new ProductImage { Id = Guid.Parse("98a6070a-3b81-4c64-b004-371a6de1e151"), ProductDetailId = Guid.Parse("3dd3a835-efb9-4656-8128-1bccf2020e95"), ImageUrl = "/uploads/product-details/3dd3a835-efb9-4656-8128-1bccf2020e95/140f050f-160d-4cfb-9f0b-00c6c55c73d9.webp" },
                new ProductImage { Id = Guid.Parse("9515e595-e10f-4bca-b70d-3d0506a70961"), ProductDetailId = Guid.Parse("26d96697-c8ca-4cb9-8314-be1d32386d74"), ImageUrl = "/uploads/product-details/26d96697-c8ca-4cb9-8314-be1d32386d74/69cb8ed7-32d2-4be0-b5cd-7a1395ee77ba.webp" },
                new ProductImage { Id = Guid.Parse("dc4a2d34-47b2-4c0a-919d-4690465841a8"), ProductDetailId = Guid.Parse("2b77eed7-80c0-42e3-9bc5-94677c01dc99"), ImageUrl = "/uploads/product-details/2b77eed7-80c0-42e3-9bc5-94677c01dc99/b2f3c83c-9cae-41d5-ad6b-019eb38379a0.webp" },
                new ProductImage { Id = Guid.Parse("862127da-a1d3-4ace-9460-480568c7770f"), ProductDetailId = Guid.Parse("014ffd65-5d70-4534-a11c-245361dafbe8"), ImageUrl = "/uploads/product-details/014ffd65-5d70-4534-a11c-245361dafbe8/44685adc-9179-40fb-a88e-f2cd8ca10d2a.webp" },
                new ProductImage { Id = Guid.Parse("3d28d9ba-8ca9-4d90-a56a-51b839722410"), ProductDetailId = Guid.Parse("d0025a02-fbd3-41b4-b444-d84c9e902e08"), ImageUrl = "/uploads/product-details/d0025a02-fbd3-41b4-b444-d84c9e902e08/5c6068c7-21f1-47f1-a103-4c44d790b4b0.webp" },
                new ProductImage { Id = Guid.Parse("42c1bd70-9159-40d2-bd33-543658b8c17b"), ProductDetailId = Guid.Parse("268d8602-8ffb-4f08-af04-309d9ee7a652"), ImageUrl = "/uploads/product-details/268d8602-8ffb-4f08-af04-309d9ee7a652/ddc35887-da66-4d19-b9f8-f4eb8dcb7685.webp" },
                new ProductImage { Id = Guid.Parse("704331b4-e712-47b6-a71e-6823e41f85f7"), ProductDetailId = Guid.Parse("8f1f78bd-aac3-4ffe-9b67-a5b0546291f9"), ImageUrl = "/uploads/product-details/8f1f78bd-aac3-4ffe-9b67-a5b0546291f9/98d8ec04-9aad-4411-aec3-6fdf49bd1a0d.webp" },
                new ProductImage { Id = Guid.Parse("5a6da4f4-f107-46a9-845d-695256ed958d"), ProductDetailId = Guid.Parse("49219172-62e3-4e6d-bbb8-57b7466a59f4"), ImageUrl = "/uploads/product-details/49219172-62e3-4e6d-bbb8-57b7466a59f4/c23bcf82-d3b1-4fbb-b202-76a7267e39ae.jpg" },
                new ProductImage { Id = Guid.Parse("31cc06d8-ab17-4a91-a195-7106cc0ece06"), ProductDetailId = Guid.Parse("e44202d7-7a1b-49ce-9573-aff799783500"), ImageUrl = "/uploads/product-details/e44202d7-7a1b-49ce-9573-aff799783500/553fcf43-717e-43e4-9f62-fc44d97eb251.webp" },
                new ProductImage { Id = Guid.Parse("4ce58328-7b9f-4f99-87ab-756b7e52438c"), ProductDetailId = Guid.Parse("81c268b2-a541-48b0-8e98-73b92512b1ad"), ImageUrl = "/uploads/product-details/81c268b2-a541-48b0-8e98-73b92512b1ad/f0edd22c-3a16-4e43-976b-dbd748d077d3.webp" },
                new ProductImage { Id = Guid.Parse("75ffaa89-9ed5-4d72-ba97-7a94b5bb1815"), ProductDetailId = Guid.Parse("3dbe0697-6b17-4854-a251-05db0b49a684"), ImageUrl = "/uploads/product-details/3dbe0697-6b17-4854-a251-05db0b49a684/bdceaaf5-b097-4b94-b961-e97c044309b2.webp" },
                new ProductImage { Id = Guid.Parse("6a5754b8-b821-4855-a931-80ec039128d3"), ProductDetailId = Guid.Parse("1d5b0c18-60f3-46a6-9df8-003211c32983"), ImageUrl = "/uploads/product-details/1d5b0c18-60f3-46a6-9df8-003211c32983/b3ca37a6-b3ef-47ae-9a09-1751336ba19f.webp" },
                new ProductImage { Id = Guid.Parse("a1ce0b3f-5fb4-4585-b064-838edc96a794"), ProductDetailId = Guid.Parse("0677c96a-7474-4936-b8d6-24bd1c1ac08b"), ImageUrl = "/uploads/product-details/0677c96a-7474-4936-b8d6-24bd1c1ac08b/4a39c052-8133-4641-87df-dbcbcc3f5ff5.webp" },
                new ProductImage { Id = Guid.Parse("0c9fdeab-fd4f-4fb3-a10e-859d695fcc69"), ProductDetailId = Guid.Parse("46a67c25-d91e-4cf6-8c24-3434dbd06a34"), ImageUrl = "/uploads/product-details/46a67c25-d91e-4cf6-8c24-3434dbd06a34/1ec7e57d-1fc3-4432-81f9-3255f77d01ef.jpg" },
                new ProductImage { Id = Guid.Parse("b813ee6d-0a91-474a-98f2-92c1d4fb0e51"), ProductDetailId = Guid.Parse("6e3d5c21-c4c2-4a99-896d-a7cf79088041"), ImageUrl = "/uploads/product-details/6e3d5c21-c4c2-4a99-896d-a7cf79088041/9d85c5ea-4219-49ef-962e-2dd2f9816d25.webp" },
                new ProductImage { Id = Guid.Parse("ba4f34ac-5578-4788-a2b4-9446b816bc6e"), ProductDetailId = Guid.Parse("754ca79f-7c36-45f3-8c82-23e2978f1b6a"), ImageUrl = "/uploads/product-details/754ca79f-7c36-45f3-8c82-23e2978f1b6a/04ba3bb2-7530-4a2d-9d58-20129a5a07c9.jpg" },
                new ProductImage { Id = Guid.Parse("999c981c-6823-42cd-aa2e-a63a0a89f3f9"), ProductDetailId = Guid.Parse("b111948c-c3e3-41ce-93c5-831b90897fd7"), ImageUrl = "/uploads/product-details/b111948c-c3e3-41ce-93c5-831b90897fd7/4f08440a-18ab-46f1-b850-8f302908ebf6.webp" },
                new ProductImage { Id = Guid.Parse("c1a149bb-f65a-4749-bf29-bd7776a20a6f"), ProductDetailId = Guid.Parse("06efd08e-338d-41aa-b6cb-ad5d63a8f412"), ImageUrl = "/uploads/product-details/06efd08e-338d-41aa-b6cb-ad5d63a8f412/6b0b0329-259c-4524-83cf-6e292315019c.webp" },
                new ProductImage { Id = Guid.Parse("3c83d4b6-89be-415b-9b9d-eb394059ad9b"), ProductDetailId = Guid.Parse("72ddf35a-f7c9-420e-9dd2-650c6082779d"), ImageUrl = "/uploads/product-details/72ddf35a-f7c9-420e-9dd2-650c6082779d/0cba5d21-f486-4eac-bb3f-150a76d9e0e6.webp" },
                new ProductImage { Id = Guid.Parse("1a82b9c8-2e0d-4237-8b4e-faf7c79c4d32"), ProductDetailId = Guid.Parse("d7dc2fc2-cf47-453a-ba5c-4f21e33fe112"), ImageUrl = "/uploads/product-details/d7dc2fc2-cf47-453a-ba5c-4f21e33fe112/b676c131-4f11-49c9-8826-b459b1142988.webp" },
                new ProductImage { Id = Guid.Parse("30449a6a-c59b-4237-a623-fcb1497ff2ac"), ProductDetailId = Guid.Parse("55f5a1e5-4168-4170-a1a0-cff392249ca2"), ImageUrl = "/uploads/product-details/55f5a1e5-4168-4170-a1a0-cff392249ca2/c079ad99-4043-4663-ac78-067135f474ee.webp" }
            };
            modelBuilder.Entity<ProductImage>().HasData(productImages);

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

            // Seed a few sample vouchers so clients can pick available ones at checkout
            // These values will be captured into migrations when you create/update a migration
            var voucherWelcomeId = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111");
            var voucherSaveId = Guid.Parse("22222222-aaaa-bbbb-cccc-222222222222");
            var voucherBigId = Guid.Parse("33333333-aaaa-bbbb-cccc-333333333333");

            modelBuilder.Entity<Voucher>().HasData(
                new Voucher
                {
                    Id = voucherWelcomeId,
                    Code = "WELCOME10",
                    DiscountPercentage = 10m,
                    MaxDiscountAmount = 50000m,
                    MinOrderAmount = 300000m,
                    StartDate = DateTime.Now.AddDays(-7),
                    EndDate = DateTime.Now.AddMonths(1),
                    UsageCount = 0,
                    MaxUsage = 1000
                },
                new Voucher
                {
                    Id = voucherSaveId,
                    Code = "SAVE15",
                    DiscountPercentage = 15m,
                    MaxDiscountAmount = 100000m,
                    MinOrderAmount = 500000m,
                    StartDate = DateTime.Now.AddDays(-3),
                    EndDate = DateTime.Now.AddMonths(2),
                    UsageCount = 0,
                    MaxUsage = 500
                },
                new Voucher
                {
                    Id = voucherBigId,
                    Code = "BIGSALE25",
                    DiscountPercentage = 25m,
                    MaxDiscountAmount = 200000m,
                    MinOrderAmount = 800000m,
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddMonths(3),
                    UsageCount = 0,
                    MaxUsage = 300
                }
            );

            // Seed PaymentMethods
            // COD - Chỉ dành cho bán Online
            // Tiền mặt, Chuyển khoản - Chỉ dành cho bán Offline/POS
            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111001"),
                    Name = "COD",
                    Description = "Thanh toán khi nhận hàng (Cash On Delivery)",
                    Channel = SalesChannel.Online
                },
                new PaymentMethod
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111002"),
                    Name = "Tiền mặt",
                    Description = "Thanh toán bằng tiền mặt tại cửa hàng",
                    Channel = SalesChannel.Offline
                },
                new PaymentMethod
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111003"),
                    Name = "Chuyển khoản",
                    Description = "Thanh toán qua chuyển khoản ngân hàng",
                    Channel = SalesChannel.Offline
                }
            };
            modelBuilder.Entity<PaymentMethod>().HasData(paymentMethods);
        }
    }
}
