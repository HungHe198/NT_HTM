using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class OrdersController : Controller
    {
        private readonly NT.BLL.Interfaces.IGenericRepository<Order> _orderRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<OrderDetail> _orderDetailRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<ProductDetail> _productDetailRepo;
        private readonly NT.WEB.Services.CustomerWebService _customerService;

        public OrdersController(NT.BLL.Interfaces.IGenericRepository<Order> orderRepo,
                                 NT.BLL.Interfaces.IGenericRepository<OrderDetail> orderDetailRepo,
                                 NT.BLL.Interfaces.IGenericRepository<ProductDetail> productDetailRepo,
                                 NT.WEB.Services.CustomerWebService customerService)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _productDetailRepo = productDetailRepo;
            _customerService = customerService;
        }

        
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index(string? status)
        {
            var orders = await _orderRepo.GetAllAsync();
            var list = orders ?? Array.Empty<Order>();
            if (!string.IsNullOrWhiteSpace(status))
            {
                list = list.Where(o => string.Equals(o.Status, status, StringComparison.Ordinal)).ToList();
            }
            ViewBag.FilterStatus = status;
            return View(list);
        }

        // Customer: view all own orders (any status)
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> My(string? status)
        {
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) return Forbid();
            var customers = await _customerService.FindAsync(c => c.UserId == userId);
            var customer = customers?.FirstOrDefault();
            if (customer == null) return Forbid();

            var orders = await _orderRepo.FindAsync(o => o.CustomerId == customer.Id);
            var list = (orders ?? Array.Empty<Order>()).ToList();
            if (!string.IsNullOrWhiteSpace(status))
            {
                list = list.Where(o => string.Equals(o.Status, status, StringComparison.Ordinal)).ToList();
            }
            ViewBag.FilterStatus = status;
            return View("Index", list);
        }

        // Customer/admin can review an order details
        [HttpGet]
        public async Task<IActionResult> Review(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var orders = await _orderRepo.FindAsync(o => o.Id == id);
            var order = orders?.FirstOrDefault();
            if (order == null) return NotFound();
            var details = await _orderDetailRepo.FindAsync(d => d.OrderId == id);
            ViewBag.Details = details ?? Array.Empty<OrderDetail>();
            // Map status code to localized display text for the Review page
            var st = order.Status ?? "0";
            string statusText = st switch
            {
                "-1" => "Đã hủy",
                "0" => "Chờ xác nhận",
                "1" => "Đã xác nhận",
                "2" => "Đang giao hàng",
                "3" => "Giao thành công",
                _ => "Không rõ"
            };
            ViewBag.StatusText = statusText;
            return View(order);
        }

        // Admin: standardized status transitions for sales project
        // Status codes (persisted):
        // -1 = Canceled, 0 = Pending, 1 = Confirmed, 2 = Shipping, 3 = Delivered
        // Display mapping for users (p2): "-1" = "đã hủy", "0" = "chờ xác nhận", "1" = "đã xác nhận", "2" = "đang vận chuyển", "3" = "giao thành công"
        // When moving from Pending (0) to Confirmed (1), decrease ProductDetail.StockQuantity and increase SoldQuantity
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid id, string status)
        {
            if (id == Guid.Empty) return BadRequest();
            var orders = await _orderRepo.FindAsync(o => o.Id == id);
            var order = orders?.FirstOrDefault();
            if (order == null) return NotFound();

            // Block any updates if already canceled
            if (string.Equals(order.Status, "-1", StringComparison.Ordinal))
            {
                TempData["Error"] = "Đơn đã hủy, không thể cập nhật trạng thái.";
                return RedirectToAction(nameof(Review), new { id });
            }

            var previousStatus = order.Status ?? "0";
            order.Status = status;

            // Inventory adjustment only on transition to Confirmed from non-confirmed states
            if (previousStatus != "1" && status == "1")
            {
                var details = await _orderDetailRepo.FindAsync(d => d.OrderId == id);
                foreach (var d in details ?? Array.Empty<OrderDetail>())
                {
                    var pd = await _productDetailRepo.GetByIdAsync(d.ProductDetailId);
                    if (pd != null)
                    {
                        // Decrease stock but not below zero, increase sold
                        var newStock = Math.Max(0, pd.StockQuantity - d.Quantity);
                        pd.StockQuantity = newStock;
                        pd.SoldQuantity = pd.SoldQuantity + d.Quantity;
                        await _productDetailRepo.UpdateAsync(pd);
                    }
                }
            }

            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();
            await _productDetailRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Review), new { id });
        }

        // Cancel order with note (admin/employee/customer). Note is required on cancel.
        [Authorize(Roles = "Admin,Employee,Customer")]
        [HttpPost]
        public async Task<IActionResult> Cancel(Guid id, string note)
        {
            if (id == Guid.Empty) return BadRequest();
            note = note?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(note))
            {
                TempData["Error"] = "Vui lòng ghi lý do hủy đơn";
                return RedirectToAction(nameof(Review), new { id });
            }
            var orders = await _orderRepo.FindAsync(o => o.Id == id);
            var order = orders?.FirstOrDefault();
            if (order == null) return NotFound();

            var previousStatus = order.Status ?? "0";
            order.Note = note; // hidden from customer during placement, only used for cancellation reason
            order.Status = "-1"; // canceled
            await _orderRepo.UpdateAsync(order);
            // If the order was previously in a status that deducted inventory, restore for each item
            if (previousStatus == "1" || previousStatus == "2" || previousStatus == "3")
            {
                var details = await _orderDetailRepo.FindAsync(d => d.OrderId == id);
                foreach (var d in details ?? Array.Empty<OrderDetail>())
                {
                    var pd = await _productDetailRepo.GetByIdAsync(d.ProductDetailId);
                    if (pd != null)
                    {
                        pd.StockQuantity = pd.StockQuantity + d.Quantity;
                        pd.SoldQuantity = Math.Max(0, pd.SoldQuantity - d.Quantity);
                        await _productDetailRepo.UpdateAsync(pd);
                    }
                }
                await _productDetailRepo.SaveChangesAsync();
            }
            await _orderRepo.SaveChangesAsync();
            TempData["Success"] = "Đã hủy đơn hàng";
            return RedirectToAction(nameof(Review), new { id });
        }
    }
}
