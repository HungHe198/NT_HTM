using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    /// <summary>
    /// Bán hàng tại quầy (POS - Point of Sale) cho nhân viên bán hàng
    /// </summary>
 [Authorize(Roles = "Admin,Employee")]
    public class SalesController : Controller
  {
        private readonly NT.BLL.Interfaces.IGenericRepository<Product> _productRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<ProductDetail> _productDetailRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<Order> _orderRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<OrderDetail> _orderDetailRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<Customer> _customerRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<PaymentMethod> _paymentMethodRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<Voucher> _voucherRepo;
    private readonly CustomerWebService _customerService;

        public SalesController(
    NT.BLL.Interfaces.IGenericRepository<Product> productRepo,
  NT.BLL.Interfaces.IGenericRepository<ProductDetail> productDetailRepo,
       NT.BLL.Interfaces.IGenericRepository<Order> orderRepo,
   NT.BLL.Interfaces.IGenericRepository<OrderDetail> orderDetailRepo,
            NT.BLL.Interfaces.IGenericRepository<Customer> customerRepo,
            NT.BLL.Interfaces.IGenericRepository<PaymentMethod> paymentMethodRepo,
            NT.BLL.Interfaces.IGenericRepository<Voucher> voucherRepo,
       CustomerWebService customerService)
        {
            _productRepo = productRepo;
     _productDetailRepo = productDetailRepo;
            _orderRepo = orderRepo;
        _orderDetailRepo = orderDetailRepo;
         _customerRepo = customerRepo;
  _paymentMethodRepo = paymentMethodRepo;
          _voucherRepo = voucherRepo;
_customerService = customerService;
      }

    // Trang chính POS
        public async Task<IActionResult> Index()
        {
        var products = await _productRepo.GetAllAsync();
         var paymentMethods = await _paymentMethodRepo.GetAllAsync();
            ViewBag.Products = products ?? new List<Product>();
    ViewBag.PaymentMethods = paymentMethods ?? new List<PaymentMethod>();
   return View();
   }

        // API: Tìm kiếm sản phẩm theo tên/mã
    [HttpGet]
      public async Task<IActionResult> SearchProducts(string q)
 {
        if (string.IsNullOrWhiteSpace(q)) 
    {
    return Json(new List<object>());
    }
     
  q = q.ToLower().Trim();
   var products = await _productRepo.GetAllAsync();
      var filtered = (products ?? Enumerable.Empty<Product>())
 .Where(p => !string.IsNullOrEmpty(p.Name) && 
    (!string.IsNullOrEmpty(p.ProductCode) &&
      (p.Name.ToLower().Contains(q) || p.ProductCode.ToLower().Contains(q))))         .Select(p => new { p.Id, p.Name, p.ProductCode })
    .Take(10)
        .ToList();
    
    return Json(filtered);
        }

        // API: Lấy biến thể sản phẩm
        [HttpGet]
  public async Task<IActionResult> GetProductVariants(Guid productId)
        {
   if (productId == Guid.Empty) return Json(new { });
    
        // Use FindAsync with includes để load Length, Hardness, Color
     var details = await _productDetailRepo.FindAsync(
        d => d.ProductId == productId && d.StockQuantity > 0,
            d => d.Length!,
        d => d.Hardness!,
      d => d.Color!
        );
   
    var data = details?
  .Select(d => new
     {
  d.Id,
d.Price,
        d.StockQuantity,
        d.ProductId,
    length = d.Length?.Name ?? "N/A",
       hardness = d.Hardness?.Name ?? "N/A",
           color = d.Color?.Name ?? "N/A"
            })
     .ToList() ?? new();
    return Json(data);
        }

 // API: Tìm khách hàng theo SĐT
      [HttpGet]
        public async Task<IActionResult> SearchCustomer(string phone)
      {
     if (string.IsNullOrWhiteSpace(phone)) return Json(null);
            var customers = await _customerService.FindAsync(c => c.User!.PhoneNumber == phone.Trim());
      var customer = customers?.FirstOrDefault();
            if (customer == null) return Json(null);
            return Json(new
          {
 customer.Id,
       name = customer.User?.Fullname,
      phone = customer.User?.PhoneNumber,
         address = customer.Address
            });
  }

        // API: Kiểm tra và apply voucher
        [HttpPost]
    public async Task<IActionResult> ValidateVoucher(string voucherCode, decimal totalAmount)
     {
        if (string.IsNullOrWhiteSpace(voucherCode)) 
      return Json(new { success = false, message = "Nhập mã giảm giá" });

            var vouchers = await _voucherRepo.FindAsync(v => v.Code == voucherCode.Trim().ToUpper());
            var voucher = vouchers?.FirstOrDefault();

            if (voucher == null)
       return Json(new { success = false, message = "Mã giảm giá không tồn tại" });

            // Kiểm tra ngày hết hạn (dùng thời gian máy chủ theo local time)
            if (voucher.EndDate.HasValue && DateTime.Now > voucher.EndDate.Value)
                return Json(new { success = false, message = "Mã voucher đã hết hạn" });

       // Kiểm tra số lần sử dụng
            if (voucher.MaxUsage.HasValue && voucher.UsageCount >= voucher.MaxUsage.Value)
       return Json(new { success = false, message = "Mã giảm giá đã hết lượt sử dụng" });

      // Kiểm tra đơn hàng tối thiểu
            if (voucher.MinOrderAmount.HasValue && totalAmount < voucher.MinOrderAmount.Value)
        return Json(new { success = false, message = $"Đơn hàng phải tối thiểu {voucher.MinOrderAmount.Value:N0}₫" });

 // Tính tiền giảm
            decimal discount = 0;
        if (voucher.DiscountPercentage.HasValue)
            {
            discount = totalAmount * (decimal)voucher.DiscountPercentage.Value / 100;
       if (voucher.MaxDiscountAmount.HasValue)
   {
           discount = Math.Min(discount, voucher.MaxDiscountAmount.Value);
     }
            }

        return Json(new
     {
    success = true,
                voucherId = voucher.Id,
   discount = discount,
    finalAmount = totalAmount - discount
            });
        }

        // Tạo đơn hàng tại quầy
      [HttpPost]
    public async Task<IActionResult> CreateOrder(
            [FromBody] SalesOrderModel model)
        {
            if (model == null || !model.Items.Any())
     return Json(new { success = false, message = "Giỏ hàng trống" });

            try
            {
   // Tạo hoặc lấy khách hàng
          Guid customerId;
    if (!string.IsNullOrWhiteSpace(model.CustomerPhone))
                {
      var customers = await _customerService.FindAsync(c => c.User!.PhoneNumber == model.CustomerPhone.Trim());
      var customer = customers?.FirstOrDefault();
       if (customer == null)
      {
     return Json(new { success = false, message = "Khách hàng không tồn tại. Vui lòng tạo khách hàng trước." });
      }
   customerId = customer.Id;
    }
      else
  {
           return Json(new { success = false, message = "Vui lòng chọn khách hàng" });
      }

    // Tính tổng tiền
        decimal totalAmount = 0;
   var orderDetails = new List<OrderDetail>();

             foreach (var item in model.Items)
                {
       var detail = await _productDetailRepo.GetByIdAsync(item.ProductDetailId);
          if (detail == null || detail.StockQuantity < item.Quantity)
   {
         return Json(new { success = false, message = $"Sản phẩm không đủ hàng hoặc không tồn tại" });
         }

    decimal itemTotal = detail.Price * item.Quantity;
            totalAmount += itemTotal;

        var orderDetail = new OrderDetail
         {
           ProductDetailId = item.ProductDetailId,
                   NameAtOrder = detail.Product?.Name,
            Quantity = item.Quantity,
           UnitPrice = detail.Price,
          LengthAtOrder = detail.Length?.Name,
  ColorAtOrder = detail.Color?.Name,
    HardnessAtOrder = detail.Hardness?.Name,
           TotalPrice = itemTotal
     };
   orderDetails.Add(orderDetail);
      }

    // Áp dụng voucher nếu có
  decimal discount = 0;
         Guid? voucherId = null;
   if (!string.IsNullOrWhiteSpace(model.VoucherCode))
                {
            var vouchers = await _voucherRepo.FindAsync(v => v.Code == model.VoucherCode.Trim().ToUpper());
            var voucher = vouchers?.FirstOrDefault();
  if (voucher != null && (!voucher.EndDate.HasValue || DateTime.Now <= voucher.EndDate.Value))
          {
     voucherId = voucher.Id;
        if (voucher.DiscountPercentage.HasValue)
{
              discount = totalAmount * (decimal)voucher.DiscountPercentage.Value / 100;
   if (voucher.MaxDiscountAmount.HasValue)
     discount = Math.Min(discount, voucher.MaxDiscountAmount.Value);
               }
   voucher.UsageCount++;
         await _voucherRepo.UpdateAsync(voucher);
         }
  }

    decimal finalAmount = totalAmount - discount;

 // Tạo đơn hàng
     var order = new Order
        {
        CustomerId = customerId,
    VoucherId = voucherId,
    PaymentMethodId = model.PaymentMethodId,
           CreatedTime = DateTime.UtcNow,
        TotalAmount = totalAmount,
        DiscountAmount = discount > 0 ? discount : null,
      FinalAmount = finalAmount,
             Status = "1", // Confirmed (đã xác nhận)
            PhoneNumber = model.CustomerPhone,
  ShippingAddress = model.ShippingAddress ?? string.Empty,
   Note = model.Note,
 CreatedByUserId = GetCurrentUserId()
 };

      await _orderRepo.AddAsync(order);
   
   // Thêm chi tiết đơn hàng
  foreach (var detail in orderDetails)
            {
          detail.OrderId = order.Id;
             await _orderDetailRepo.AddAsync(detail);
                }

   // Cập nhật tồn kho
        foreach (var item in model.Items)
     {
    var detail = await _productDetailRepo.GetByIdAsync(item.ProductDetailId);
  if (detail != null)
         {
              detail.StockQuantity -= item.Quantity;
         detail.SoldQuantity += item.Quantity;
          await _productDetailRepo.UpdateAsync(detail);
     }
              }

     await _orderRepo.SaveChangesAsync();
     await _orderDetailRepo.SaveChangesAsync();
        await _productDetailRepo.SaveChangesAsync();
   await _voucherRepo.SaveChangesAsync();

    return Json(new
    {
         success = true,
  message = "Tạo đơn hàng thành công",
   orderId = order.Id,
        orderCode = $"POS-{order.Id.ToString().Substring(0, 8).ToUpper()}",
    finalAmount = order.FinalAmount
         });
    }
            catch (Exception ex)
    {
        return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

      // In hóa đơn
        [HttpGet]
        public async Task<IActionResult> PrintReceipt(Guid orderId)
        {
     if (orderId == Guid.Empty) return BadRequest();
var order = await _orderRepo.GetByIdAsync(orderId);
  if (order == null) return NotFound();

        var details = await _orderDetailRepo.FindAsync(d => d.OrderId == orderId);
  ViewBag.Details = details ?? new List<OrderDetail>();
            ViewBag.OrderCode = $"POS-{orderId.ToString().Substring(0, 8).ToUpper()}";
            return View(order);
        }

     // Lịch sử bán hàng tại quầy
   [HttpGet]
public async Task<IActionResult> History(int days = 30)
     {
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
 if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) 
      return Forbid();

         var from = DateTime.UtcNow.Date.AddDays(-days);
      var orders = await _orderRepo.FindAsync(o => o.CreatedByUserId == userId && o.CreatedTime >= from);
         var list = (orders ?? Array.Empty<Order>()).OrderByDescending(o => o.CreatedTime).ToList();
            ViewBag.Days = days;
            return View(list);
        }

        // Helper: Lấy ID user hiện tại
        private Guid GetCurrentUserId()
        {
 var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }

    /// <summary>
    /// Model nhận từ frontend cho tạo đơn hàng tại quầy
    /// </summary>
    public class SalesOrderModel
    {
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }
   public string VoucherCode { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string Note { get; set; }
      public List<SalesOrderItem> Items { get; set; } = new();
    }

    public class SalesOrderItem
    {
    public Guid ProductDetailId { get; set; }
        public int Quantity { get; set; }
    }
}
