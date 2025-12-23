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
        private readonly NT.BLL.Interfaces.IGenericRepository<Voucher> _voucherRepo;
        private readonly NT.WEB.Services.CustomerWebService _customerService;

        public OrdersController(NT.BLL.Interfaces.IGenericRepository<Order> orderRepo,
                                 NT.BLL.Interfaces.IGenericRepository<OrderDetail> orderDetailRepo,
                                 NT.BLL.Interfaces.IGenericRepository<ProductDetail> productDetailRepo,
                                 NT.BLL.Interfaces.IGenericRepository<Voucher> voucherRepo,
                                 NT.WEB.Services.CustomerWebService customerService)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _productDetailRepo = productDetailRepo;
            _voucherRepo = voucherRepo;
            _customerService = customerService;
        }

        
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index(string? status, string? q)
        {
            // Include Customer and nested User so that the view can show customer name
            var orders = await _orderRepo.FindAsync(o => true,
                                                    o => o.Customer!,
                                                    o => o.Customer!.User!);
            var list = orders ?? Array.Empty<Order>();
            if (!string.IsNullOrWhiteSpace(status))
            {
                list = list.Where(o => string.Equals(o.Status, status, StringComparison.Ordinal)).ToList();
            }
            // Filter by phone number (order phone or customer's phone) if query provided
            if (!string.IsNullOrWhiteSpace(q))
            {
                var query = q.Trim().ToLowerInvariant();
                list = list.Where(o =>
                    (!string.IsNullOrWhiteSpace(o.PhoneNumber) && o.PhoneNumber.ToLowerInvariant().Contains(query)) ||
                    (!string.IsNullOrWhiteSpace(o.Customer?.User?.PhoneNumber) && o.Customer!.User!.PhoneNumber!.ToLowerInvariant().Contains(query))
                ).ToList();
            }
            ViewBag.FilterStatus = status;
            ViewBag.Query = q;
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

            // Kiểm tra tồn kho trước khi xác nhận đơn hàng (chuyển từ 0 sang 1)
            // Điều này đảm bảo không xảy ra tình trạng xác nhận đơn online khi tồn kho đã bị trừ bởi đơn POS chờ
            if (previousStatus != "1" && status == "1")
            {
                var details = await _orderDetailRepo.FindAsync(d => d.OrderId == id);
                var insufficientStockItems = new System.Collections.Generic.List<string>();

                foreach (var d in details ?? Array.Empty<OrderDetail>())
                {
                    var pd = await _productDetailRepo.GetByIdAsync(d.ProductDetailId);
                    if (pd != null)
                    {
                        if (pd.StockQuantity < d.Quantity)
                        {
                            var productName = d.NameAtOrder ?? "Sản phẩm";
                            var variantInfo = $"{d.LengthAtOrder ?? ""} - {d.HardnessAtOrder ?? ""} - {d.ColorAtOrder ?? ""}".Trim(' ', '-');
                            if (!string.IsNullOrWhiteSpace(variantInfo))
                            {
                                productName += $" ({variantInfo})";
                            }
                            insufficientStockItems.Add($"• {productName}: Yêu cầu {d.Quantity}, Tồn kho còn {pd.StockQuantity}");
                        }
                    }
                    else
                    {
                        insufficientStockItems.Add($"• {d.NameAtOrder ?? "Sản phẩm"}: Không tìm thấy sản phẩm trong hệ thống");
                    }
                }

                if (insufficientStockItems.Any())
                {
                    TempData["Error"] = $"Không thể xác nhận đơn hàng do không đủ tồn kho:\n{string.Join("\n", insufficientStockItems)}";
                    return RedirectToAction(nameof(Review), new { id });
                }
            }

            order.Status = status;

            // Lưu thông tin người thực hiện hành động theo từng trạng thái
            var currentUserId = GetCurrentUserId();
            switch (status)
            {
                case "1": // Confirmed - Nhân viên xác nhận đơn
                    order.ConfirmedByUserId = currentUserId;
                    break;
                case "2": // Shipping - Nhân viên bàn giao đơn
                    order.HandoverByUserId = currentUserId;
                    break;
                case "3": // Delivered - Nhân viên xác nhận hoàn thành
                    order.CompletedByUserId = currentUserId;
                    break;
            }

            // Inventory adjustment only on transition to Confirmed from non-confirmed states
            if (previousStatus != "1" && status == "1")
            {
                var details = await _orderDetailRepo.FindAsync(d => d.OrderId == id);
                foreach (var d in details ?? Array.Empty<OrderDetail>())
                {
                    var pd = await _productDetailRepo.GetByIdAsync(d.ProductDetailId);
                    if (pd != null)
                    {
                        // Decrease stock, increase sold
                        pd.StockQuantity -= d.Quantity;
                        pd.SoldQuantity += d.Quantity;
                        await _productDetailRepo.UpdateAsync(pd);
                    }
                }
            }

            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();
            await _productDetailRepo.SaveChangesAsync();
            TempData["Success"] = "Cập nhật trạng thái đơn hàng thành công.";
            return RedirectToAction(nameof(Review), new { id });
        }

        /// <summary>
        /// Lấy ID của user đang đăng nhập
        /// </summary>
        private Guid GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
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
            order.CancelledByUserId = GetCurrentUserId(); // Lưu người hủy đơn
            await _orderRepo.UpdateAsync(order);

            // Hoàn trả UsageCount của voucher nếu đơn hàng có sử dụng voucher
            if (order.VoucherId.HasValue)
            {
                var voucher = await _voucherRepo.GetByIdAsync(order.VoucherId.Value);
                if (voucher != null && voucher.UsageCount > 0)
                {
                    voucher.UsageCount--;
                    await _voucherRepo.UpdateAsync(voucher);
                    await _voucherRepo.SaveChangesAsync();
                }
            }

            // If the order was previously in a status that deducted inventory, restore for each item
            // But skip restock when reason indicates seller-confirmed lost/damaged goods
            var normalizedNote = (order.Note ?? string.Empty).ToLowerInvariant();
            var isLostOrDamaged = normalizedNote.Contains("mất hàng") || normalizedNote.Contains("hỏng hàng");
            if ((previousStatus == "1" || previousStatus == "2" || previousStatus == "3") && !isLostOrDamaged)
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
