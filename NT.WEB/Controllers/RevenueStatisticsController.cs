using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NT.WEB.Services;
using System;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    /// <summary>
    /// Controller thống kê doanh thu
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class RevenueStatisticsController : Controller
    {
        private readonly RevenueStatisticsWebService _statisticsService;

        public RevenueStatisticsController(RevenueStatisticsWebService statisticsService)
        {
            _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        }

        /// <summary>
        /// Trang chính thống kê doanh thu
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(int? year, int? month)
        {
            var selectedYear = year ?? DateTime.Now.Year;
            var selectedMonth = month ?? DateTime.Now.Month;

            var model = await _statisticsService.GetStatisticsAsync(selectedYear, selectedMonth);

            // Lấy danh sách năm để hiển thị dropdown
            ViewBag.AvailableYears = await _statisticsService.GetAvailableYearsAsync();

            return View(model);
        }

        /// <summary>
        /// API lấy dữ liệu thống kê dạng JSON (cho AJAX refresh)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatistics(int year, int month)
        {
            try
            {
                var model = await _statisticsService.GetStatisticsAsync(year, month);
                return Json(new { success = true, data = model });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// API lấy doanh thu theo danh mục
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRevenueByCategory(int year, int month)
        {
            try
            {
                var model = await _statisticsService.GetStatisticsAsync(year, month);
                return Json(new { success = true, data = model.RevenueByCategory });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// API lấy top sản phẩm bán chạy
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTopProducts(int year, int month, int top = 10)
        {
            try
            {
                var model = await _statisticsService.GetStatisticsAsync(year, month);
                var data = model.TopProducts.Take(top);
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// API lấy sản phẩm doanh thu cao nhất mỗi ngày
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDailyTopProducts(int year, int month)
        {
            try
            {
                var model = await _statisticsService.GetStatisticsAsync(year, month);
                return Json(new { success = true, data = model.DailyTopProducts });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// API lấy doanh thu theo ngày trong tháng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDailyRevenue(int year, int month)
        {
            try
            {
                var model = await _statisticsService.GetStatisticsAsync(year, month);
                return Json(new { success = true, data = model.DailyRevenue });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// API lấy doanh thu theo tháng (12 tháng gần nhất)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMonthlyRevenue(int year, int month)
        {
            try
            {
                var model = await _statisticsService.GetStatisticsAsync(year, month);
                return Json(new { 
                    success = true, 
                    data = model.MonthlyRevenue,
                    growth = model.MonthlyGrowth
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
