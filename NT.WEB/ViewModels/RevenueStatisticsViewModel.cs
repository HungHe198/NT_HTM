using System;
using System.Collections.Generic;

namespace NT.WEB.ViewModels
{
    /// <summary>
    /// ViewModel tổng quan thống kê doanh thu
    /// </summary>
    public class RevenueStatisticsViewModel
    {
        /// <summary>
        /// Tổng doanh thu (tất cả thời gian)
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// Doanh thu tháng hiện tại
        /// </summary>
        public decimal CurrentMonthRevenue { get; set; }

        /// <summary>
        /// Tổng số đơn hàng (đã hoàn thành)
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Số đơn hàng tháng hiện tại
        /// </summary>
        public int CurrentMonthOrders { get; set; }

        /// <summary>
        /// Tỷ lệ tăng trưởng doanh thu so với tháng trước (%)
        /// </summary>
        public decimal RevenueGrowthRate { get; set; }

        /// <summary>
        /// ROS (Return on Sales) - Tỷ suất lợi nhuận trên doanh thu (%)
        /// </summary>
        public decimal ROS { get; set; }

        /// <summary>
        /// Năm đang xem
        /// </summary>
        public int SelectedYear { get; set; }

        /// <summary>
        /// Tháng đang xem
        /// </summary>
        public int SelectedMonth { get; set; }

        /// <summary>
        /// Doanh thu theo nhóm sản phẩm (Category)
        /// </summary>
        public List<CategoryRevenueItem> RevenueByCategory { get; set; } = new();

        /// <summary>
        /// Lợi nhuận theo nhóm sản phẩm
        /// </summary>
        public List<CategoryProfitItem> ProfitByCategory { get; set; } = new();

        /// <summary>
        /// Doanh thu theo tháng (12 tháng gần nhất)
        /// </summary>
        public List<MonthlyRevenueItem> MonthlyRevenue { get; set; } = new();

        /// <summary>
        /// Tăng trưởng doanh thu theo tháng (%)
        /// </summary>
        public List<MonthlyGrowthItem> MonthlyGrowth { get; set; } = new();

        /// <summary>
        /// Top sản phẩm bán chạy nhất
        /// </summary>
        public List<TopProductItem> TopProducts { get; set; } = new();

        /// <summary>
        /// Doanh thu theo ngày trong tháng
        /// </summary>
        public List<DailyRevenueItem> DailyRevenue { get; set; } = new();

        /// <summary>
        /// Sản phẩm doanh thu cao nhất mỗi ngày trong tháng
        /// </summary>
        public List<DailyTopProductItem> DailyTopProducts { get; set; } = new();
    }

    /// <summary>
    /// Doanh thu theo danh mục sản phẩm
    /// </summary>
    public class CategoryRevenueItem
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    /// <summary>
    /// Lợi nhuận theo danh mục sản phẩm
    /// </summary>
    public class CategoryProfitItem
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Profit { get; set; }
    }

    /// <summary>
    /// Doanh thu theo tháng
    /// </summary>
    public class MonthlyRevenueItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    /// <summary>
    /// Tăng trưởng doanh thu theo tháng
    /// </summary>
    public class MonthlyGrowthItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal GrowthRate { get; set; }
    }

    /// <summary>
    /// Top sản phẩm bán chạy
    /// </summary>
    public class TopProductItem
    {
        public Guid ProductId { get; set; }
        public Guid ProductDetailId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    /// <summary>
    /// Doanh thu theo ngày
    /// </summary>
    public class DailyRevenueItem
    {
        public DateTime Date { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    /// <summary>
    /// Sản phẩm doanh thu cao nhất trong ngày
    /// </summary>
    public class DailyTopProductItem
    {
        public DateTime Date { get; set; }
        public string DateLabel { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public Guid ProductDetailId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }
}
