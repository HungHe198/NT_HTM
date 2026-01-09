using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using NT.WEB.ViewModels;

namespace NT.WEB.Services
{
    /// <summary>
    /// Service thống kê doanh thu - kết hợp dữ liệu từ Orders, OrderDetail, Product, ProductDetail
    /// </summary>
    public class RevenueStatisticsWebService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductDetail> _productDetailRepo;
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepo;

        public RevenueStatisticsWebService(
            IGenericRepository<Order> orderRepo,
            IGenericRepository<OrderDetail> orderDetailRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductDetail> productDetailRepo,
            IGenericRepository<Category> categoryRepo,
            IGenericRepository<ProductCategory> productCategoryRepo)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _orderDetailRepo = orderDetailRepo ?? throw new ArgumentNullException(nameof(orderDetailRepo));
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _productDetailRepo = productDetailRepo ?? throw new ArgumentNullException(nameof(productDetailRepo));
            _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
            _productCategoryRepo = productCategoryRepo ?? throw new ArgumentNullException(nameof(productCategoryRepo));
        }

        /// <summary>
        /// Lấy thống kê doanh thu tổng quan
        /// </summary>
        public async Task<RevenueStatisticsViewModel> GetStatisticsAsync(int year, int month)
        {
            var model = new RevenueStatisticsViewModel
            {
                SelectedYear = year,
                SelectedMonth = month
            };

            // Lấy tất cả đơn hàng đã hoàn thành (Status = "3" là giao thành công)
            var allOrders = await _orderRepo.FindAsync(o => o.Status == "3");
            var ordersList = allOrders?.ToList() ?? new List<Order>();

            // Tổng doanh thu
            model.TotalRevenue = ordersList.Sum(o => o.FinalAmount);
            model.TotalOrders = ordersList.Count;

            // Doanh thu tháng hiện tại
            var currentMonthOrders = ordersList
                .Where(o => o.CreatedTime.Year == year && o.CreatedTime.Month == month)
                .ToList();
            model.CurrentMonthRevenue = currentMonthOrders.Sum(o => o.FinalAmount);
            model.CurrentMonthOrders = currentMonthOrders.Count;

            // Tính tỷ lệ tăng trưởng so với tháng trước
            var prevMonth = month == 1 ? 12 : month - 1;
            var prevYear = month == 1 ? year - 1 : year;
            var prevMonthOrders = ordersList
                .Where(o => o.CreatedTime.Year == prevYear && o.CreatedTime.Month == prevMonth)
                .ToList();
            var prevMonthRevenue = prevMonthOrders.Sum(o => o.FinalAmount);
            
            if (prevMonthRevenue > 0)
            {
                model.RevenueGrowthRate = Math.Round(((model.CurrentMonthRevenue - prevMonthRevenue) / prevMonthRevenue) * 100, 2);
            }

            // Tính ROS (sử dụng CostPrice từ ProductDetail)
            model.ROS = await CalculateROSAsync(ordersList);

            // Doanh thu theo danh mục
            model.RevenueByCategory = await GetRevenueByCategoryAsync(year, month);

            // Lợi nhuận theo danh mục
            model.ProfitByCategory = await GetProfitByCategoryAsync(year, month);

            // Doanh thu theo kênh bán hàng (POS và Online) 12 tháng gần nhất
            var (posRevenue, onlineRevenue) = GetRevenueByChannel(ordersList, year, month);
            model.POSRevenue = posRevenue;
            model.OnlineRevenue = onlineRevenue;

            // Tổng doanh thu POS và Online trong tháng đang chọn
            model.CurrentMonthPOSRevenue = posRevenue.LastOrDefault()?.Revenue ?? 0;
            model.CurrentMonthOnlineRevenue = onlineRevenue.LastOrDefault()?.Revenue ?? 0;

            // Doanh thu 12 tháng gần nhất
            model.MonthlyRevenue = GetMonthlyRevenue(ordersList, year, month);

            // Tăng trưởng theo tháng
            model.MonthlyGrowth = CalculateMonthlyGrowth(model.MonthlyRevenue);

            // Top sản phẩm bán chạy trong tháng
            model.TopProducts = await GetTopProductsAsync(year, month, 10);

            // Doanh thu theo ngày trong tháng
            model.DailyRevenue = GetDailyRevenue(ordersList, year, month);

            // Sản phẩm doanh thu cao nhất mỗi ngày
            model.DailyTopProducts = await GetDailyTopProductsAsync(year, month);

            return model;
        }

        /// <summary>
        /// Tính ROS (Return on Sales) = Lợi nhuận / Doanh thu * 100
        /// </summary>
        private async Task<decimal> CalculateROSAsync(List<Order> orders)
        {
            if (!orders.Any()) return 0;

            var orderIds = orders.Select(o => o.Id).ToHashSet();
            var allOrderDetails = await _orderDetailRepo.GetAllAsync();
            var relevantDetails = allOrderDetails?
                .Where(od => orderIds.Contains(od.OrderId))
                .ToList() ?? new List<OrderDetail>();

            if (!relevantDetails.Any()) return 0;

            decimal totalRevenue = 0;
            decimal totalCost = 0;

            var productDetailIds = relevantDetails.Select(od => od.ProductDetailId).Distinct().ToList();
            var productDetails = await _productDetailRepo.GetAllAsync();
            var productDetailDict = productDetails?
                .Where(pd => productDetailIds.Contains(pd.Id))
                .ToDictionary(pd => pd.Id) ?? new Dictionary<Guid, ProductDetail>();

            foreach (var od in relevantDetails)
            {
                totalRevenue += od.TotalPrice;
                if (productDetailDict.TryGetValue(od.ProductDetailId, out var pd) && pd.CostPrice.HasValue)
                {
                    totalCost += pd.CostPrice.Value * od.Quantity;
                }
            }

            if (totalRevenue == 0) return 0;
            var profit = totalRevenue - totalCost;
            return Math.Round((profit / totalRevenue) * 100, 2);
        }

        /// <summary>
        /// Doanh thu theo danh mục sản phẩm
        /// </summary>
        private async Task<List<CategoryRevenueItem>> GetRevenueByCategoryAsync(int year, int month)
        {
            var result = new List<CategoryRevenueItem>();

            var categories = await _categoryRepo.GetAllAsync();
            var categoryList = categories?.ToList() ?? new List<Category>();

            var productCategories = await _productCategoryRepo.GetAllAsync();
            var productCategoryList = productCategories?.ToList() ?? new List<ProductCategory>();

            var products = await _productRepo.GetAllAsync();
            var productDict = products?.ToDictionary(p => p.Id) ?? new Dictionary<Guid, Product>();

            var productDetails = await _productDetailRepo.GetAllAsync();
            var productDetailDict = productDetails?.ToDictionary(pd => pd.Id) ?? new Dictionary<Guid, ProductDetail>();

            // Lấy đơn hàng hoàn thành trong tháng
            var orders = await _orderRepo.FindAsync(o => 
                o.Status == "3" && 
                o.CreatedTime.Year == year && 
                o.CreatedTime.Month == month);
            var orderIds = orders?.Select(o => o.Id).ToHashSet() ?? new HashSet<Guid>();

            var orderDetails = await _orderDetailRepo.GetAllAsync();
            var relevantOrderDetails = orderDetails?
                .Where(od => orderIds.Contains(od.OrderId))
                .ToList() ?? new List<OrderDetail>();

            // Map ProductDetail -> Product -> Categories
            foreach (var category in categoryList)
            {
                var productIdsInCategory = productCategoryList
                    .Where(pc => pc.CategoryId == category.Id)
                    .Select(pc => pc.ProductId)
                    .ToHashSet();

                var productDetailIdsInCategory = productDetailDict.Values
                    .Where(pd => productIdsInCategory.Contains(pd.ProductId))
                    .Select(pd => pd.Id)
                    .ToHashSet();

                var categoryRevenue = relevantOrderDetails
                    .Where(od => productDetailIdsInCategory.Contains(od.ProductDetailId))
                    .Sum(od => od.TotalPrice);

                result.Add(new CategoryRevenueItem
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name ?? "Không xác định",
                    Revenue = categoryRevenue
                });
            }

            return result.OrderByDescending(r => r.Revenue).ToList();
        }

        /// <summary>
        /// Lợi nhuận theo danh mục sản phẩm
        /// </summary>
        private async Task<List<CategoryProfitItem>> GetProfitByCategoryAsync(int year, int month)
        {
            var result = new List<CategoryProfitItem>();

            var categories = await _categoryRepo.GetAllAsync();
            var categoryList = categories?.ToList() ?? new List<Category>();

            var productCategories = await _productCategoryRepo.GetAllAsync();
            var productCategoryList = productCategories?.ToList() ?? new List<ProductCategory>();

            var productDetails = await _productDetailRepo.GetAllAsync();
            var productDetailDict = productDetails?.ToDictionary(pd => pd.Id) ?? new Dictionary<Guid, ProductDetail>();

            var orders = await _orderRepo.FindAsync(o => 
                o.Status == "3" && 
                o.CreatedTime.Year == year && 
                o.CreatedTime.Month == month);
            var orderIds = orders?.Select(o => o.Id).ToHashSet() ?? new HashSet<Guid>();

            var orderDetails = await _orderDetailRepo.GetAllAsync();
            var relevantOrderDetails = orderDetails?
                .Where(od => orderIds.Contains(od.OrderId))
                .ToList() ?? new List<OrderDetail>();

            foreach (var category in categoryList)
            {
                var productIdsInCategory = productCategoryList
                    .Where(pc => pc.CategoryId == category.Id)
                    .Select(pc => pc.ProductId)
                    .ToHashSet();

                var productDetailIdsInCategory = productDetailDict.Values
                    .Where(pd => productIdsInCategory.Contains(pd.ProductId))
                    .Select(pd => pd.Id)
                    .ToHashSet();

                decimal revenue = 0;
                decimal cost = 0;

                foreach (var od in relevantOrderDetails.Where(od => productDetailIdsInCategory.Contains(od.ProductDetailId)))
                {
                    revenue += od.TotalPrice;
                    if (productDetailDict.TryGetValue(od.ProductDetailId, out var pd) && pd.CostPrice.HasValue)
                    {
                        cost += pd.CostPrice.Value * od.Quantity;
                    }
                }

                result.Add(new CategoryProfitItem
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name ?? "Không xác định",
                    Profit = revenue - cost
                });
            }

            return result.OrderByDescending(r => r.Profit).ToList();
        }

        /// <summary>
        /// Doanh thu 12 tháng gần nhất
        /// </summary>
        private List<MonthlyRevenueItem> GetMonthlyRevenue(List<Order> orders, int year, int month)
        {
            var result = new List<MonthlyRevenueItem>();

            for (int i = 11; i >= 0; i--)
            {
                var targetDate = new DateTime(year, month, 1).AddMonths(-i);
                var monthRevenue = orders
                    .Where(o => o.CreatedTime.Year == targetDate.Year && o.CreatedTime.Month == targetDate.Month)
                    .Sum(o => o.FinalAmount);

                result.Add(new MonthlyRevenueItem
                {
                    Year = targetDate.Year,
                    Month = targetDate.Month,
                    Label = $"T{targetDate.Month:D2}-{targetDate.Year}",
                    Revenue = monthRevenue
                });
            }

            return result;
        }

        /// <summary>
        /// Doanh thu theo kênh bán hàng (POS và Online) 12 tháng gần nhất
        /// POS: Đơn hàng có Note chứa "Bán hàng tại quầy" hoặc "POS"
        /// Online: Đơn hàng không có Note hoặc Note không chứa "Bán hàng tại quầy" và "POS"
        /// </summary>
        private (List<ChannelRevenueItem> posRevenue, List<ChannelRevenueItem> onlineRevenue) GetRevenueByChannel(List<Order> orders, int year, int month)
        {
            var posResult = new List<ChannelRevenueItem>();
            var onlineResult = new List<ChannelRevenueItem>();

            for (int i = 11; i >= 0; i--)
            {
                var targetDate = new DateTime(year, month, 1).AddMonths(-i);
                var monthOrders = orders
                    .Where(o => o.CreatedTime.Year == targetDate.Year && o.CreatedTime.Month == targetDate.Month)
                    .ToList();

                // Phân loại đơn hàng POS và Online
                var posOrders = monthOrders
                    .Where(o => !string.IsNullOrWhiteSpace(o.Note) && 
                               (o.Note.Contains("Bán hàng tại quầy", StringComparison.OrdinalIgnoreCase) || 
                                o.Note.Contains("POS", StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                var onlineOrders = monthOrders
                    .Where(o => string.IsNullOrWhiteSpace(o.Note) || 
                               (!o.Note.Contains("Bán hàng tại quầy", StringComparison.OrdinalIgnoreCase) && 
                                !o.Note.Contains("POS", StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                posResult.Add(new ChannelRevenueItem
                {
                    Year = targetDate.Year,
                    Month = targetDate.Month,
                    Label = $"T{targetDate.Month:D2}",
                    Revenue = posOrders.Sum(o => o.FinalAmount),
                    OrderCount = posOrders.Count
                });

                onlineResult.Add(new ChannelRevenueItem
                {
                    Year = targetDate.Year,
                    Month = targetDate.Month,
                    Label = $"T{targetDate.Month:D2}",
                    Revenue = onlineOrders.Sum(o => o.FinalAmount),
                    OrderCount = onlineOrders.Count
                });
            }

            return (posResult, onlineResult);
        }

        /// <summary>
        /// Tính tăng trưởng doanh thu theo tháng
        /// </summary>
        private List<MonthlyGrowthItem> CalculateMonthlyGrowth(List<MonthlyRevenueItem> monthlyRevenue)
        {
            var result = new List<MonthlyGrowthItem>();

            for (int i = 0; i < monthlyRevenue.Count; i++)
            {
                var current = monthlyRevenue[i];
                decimal growthRate = 0;

                if (i > 0)
                {
                    var previous = monthlyRevenue[i - 1];
                    if (previous.Revenue > 0)
                    {
                        growthRate = Math.Round(((current.Revenue - previous.Revenue) / previous.Revenue) * 100, 2);
                    }
                }

                result.Add(new MonthlyGrowthItem
                {
                    Year = current.Year,
                    Month = current.Month,
                    Label = current.Label,
                    GrowthRate = growthRate
                });
            }

            return result;
        }

        /// <summary>
        /// Top sản phẩm bán chạy nhất trong tháng
        /// </summary>
        private async Task<List<TopProductItem>> GetTopProductsAsync(int year, int month, int top = 10)
        {
            var orders = await _orderRepo.FindAsync(o => 
                o.Status == "3" && 
                o.CreatedTime.Year == year && 
                o.CreatedTime.Month == month);
            var orderIds = orders?.Select(o => o.Id).ToHashSet() ?? new HashSet<Guid>();

            var orderDetails = await _orderDetailRepo.GetAllAsync();
            var relevantOrderDetails = orderDetails?
                .Where(od => orderIds.Contains(od.OrderId))
                .ToList() ?? new List<OrderDetail>();

            var products = await _productRepo.GetAllAsync();
            var productDict = products?.ToDictionary(p => p.Id) ?? new Dictionary<Guid, Product>();

            var productDetails = await _productDetailRepo.GetAllAsync();
            var productDetailDict = productDetails?.ToDictionary(pd => pd.Id) ?? new Dictionary<Guid, ProductDetail>();

            var grouped = relevantOrderDetails
                .GroupBy(od => od.ProductDetailId)
                .Select(g => new
                {
                    ProductDetailId = g.Key,
                    QuantitySold = g.Sum(od => od.Quantity),
                    Revenue = g.Sum(od => od.TotalPrice)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(top)
                .ToList();

            var result = new List<TopProductItem>();
            foreach (var item in grouped)
            {
                if (productDetailDict.TryGetValue(item.ProductDetailId, out var pd))
                {
                    productDict.TryGetValue(pd.ProductId, out var product);
                    result.Add(new TopProductItem
                    {
                        ProductId = pd.ProductId,
                        ProductDetailId = item.ProductDetailId,
                        ProductName = product?.Name ?? pd.Product?.Name ?? "Không xác định",
                        ProductCode = product?.ProductCode ?? "N/A",
                        QuantitySold = item.QuantitySold,
                        Revenue = item.Revenue
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Doanh thu theo ngày trong tháng
        /// </summary>
        private List<DailyRevenueItem> GetDailyRevenue(List<Order> orders, int year, int month)
        {
            var result = new List<DailyRevenueItem>();
            var daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var targetDate = new DateTime(year, month, day);
                var dayOrders = orders
                    .Where(o => o.CreatedTime.Date == targetDate.Date)
                    .ToList();

                result.Add(new DailyRevenueItem
                {
                    Date = targetDate,
                    Label = $"{day:D2}/{month:D2}",
                    Revenue = dayOrders.Sum(o => o.FinalAmount),
                    OrderCount = dayOrders.Count
                });
            }

            return result;
        }

        /// <summary>
        /// Sản phẩm doanh thu cao nhất mỗi ngày trong tháng
        /// </summary>
        private async Task<List<DailyTopProductItem>> GetDailyTopProductsAsync(int year, int month)
        {
            var result = new List<DailyTopProductItem>();
            var daysInMonth = DateTime.DaysInMonth(year, month);

            var orders = await _orderRepo.FindAsync(o => 
                o.Status == "3" && 
                o.CreatedTime.Year == year && 
                o.CreatedTime.Month == month);
            var ordersList = orders?.ToList() ?? new List<Order>();

            var orderDetails = await _orderDetailRepo.GetAllAsync();
            var orderDetailsList = orderDetails?.ToList() ?? new List<OrderDetail>();

            var products = await _productRepo.GetAllAsync();
            var productDict = products?.ToDictionary(p => p.Id) ?? new Dictionary<Guid, Product>();

            var productDetails = await _productDetailRepo.GetAllAsync();
            var productDetailDict = productDetails?.ToDictionary(pd => pd.Id) ?? new Dictionary<Guid, ProductDetail>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var targetDate = new DateTime(year, month, day);
                var dayOrderIds = ordersList
                    .Where(o => o.CreatedTime.Date == targetDate.Date)
                    .Select(o => o.Id)
                    .ToHashSet();

                if (!dayOrderIds.Any())
                {
                    result.Add(new DailyTopProductItem
                    {
                        Date = targetDate,
                        DateLabel = $"{day:D2}/{month:D2}",
                        ProductName = "Không có đơn hàng",
                        Revenue = 0
                    });
                    continue;
                }

                var dayOrderDetails = orderDetailsList
                    .Where(od => dayOrderIds.Contains(od.OrderId))
                    .ToList();

                var topProduct = dayOrderDetails
                    .GroupBy(od => od.ProductDetailId)
                    .Select(g => new
                    {
                        ProductDetailId = g.Key,
                        QuantitySold = g.Sum(od => od.Quantity),
                        Revenue = g.Sum(od => od.TotalPrice)
                    })
                    .OrderByDescending(x => x.Revenue)
                    .FirstOrDefault();

                if (topProduct != null && productDetailDict.TryGetValue(topProduct.ProductDetailId, out var pd))
                {
                    productDict.TryGetValue(pd.ProductId, out var product);
                    result.Add(new DailyTopProductItem
                    {
                        Date = targetDate,
                        DateLabel = $"{day:D2}/{month:D2}",
                        ProductId = pd.ProductId,
                        ProductDetailId = topProduct.ProductDetailId,
                        ProductName = product?.Name ?? "Không xác định",
                        ProductCode = product?.ProductCode ?? "N/A",
                        QuantitySold = topProduct.QuantitySold,
                        Revenue = topProduct.Revenue
                    });
                }
                else
                {
                    result.Add(new DailyTopProductItem
                    {
                        Date = targetDate,
                        DateLabel = $"{day:D2}/{month:D2}",
                        ProductName = "Không có dữ liệu",
                        Revenue = 0
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Lấy danh sách năm có dữ liệu đơn hàng
        /// </summary>
        public async Task<List<int>> GetAvailableYearsAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            var years = orders?
                .Select(o => o.CreatedTime.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList() ?? new List<int>();

            if (!years.Any())
            {
                years.Add(DateTime.Now.Year);
            }

            return years;
        }
    }
}
