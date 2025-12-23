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
    [Authorize(Roles = "Customer")]
    public class CheckoutController : Controller
    {
        private readonly CartDetailWebService _cartDetailService;
        private readonly CustomerWebService _customerService;
        private readonly ProductDetailWebService _productDetailService;
        private readonly NT.BLL.Interfaces.IGenericRepository<Order> _orderRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<OrderDetail> _orderDetailRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<PaymentMethod> _paymentMethodRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<Voucher> _voucherRepo;

        public CheckoutController(
            CartDetailWebService cartDetailService,
            CustomerWebService customerService,
            ProductDetailWebService productDetailService,
            NT.BLL.Interfaces.IGenericRepository<Order> orderRepo,
            NT.BLL.Interfaces.IGenericRepository<OrderDetail> orderDetailRepo,
            NT.BLL.Interfaces.IGenericRepository<PaymentMethod> paymentMethodRepo,
            NT.BLL.Interfaces.IGenericRepository<Voucher> voucherRepo)
        {
            _cartDetailService = cartDetailService;
            _customerService = customerService;
            _productDetailService = productDetailService;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _paymentMethodRepo = paymentMethodRepo;
            _voucherRepo = voucherRepo;
        }

        // GET: /Checkout/Start?cartId=...&selected=commaSeparatedProductDetailIds
        [HttpGet]
        public async Task<IActionResult> Start(Guid cartId, string selected)
        {
            if (cartId == Guid.Empty) return BadRequest();
            var ids = (selected ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Guid.TryParse(s, out var g) ? g : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToArray();
            if (ids.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn sản phẩm để thanh toán";
                return Redirect($"/CartDetail?cartId={cartId}");
            }

            // Lấy thông tin khách hàng đang đăng nhập để điền mặc định vào form
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            string defaultPhone = "";
            string defaultAddress = "";
            string defaultName = "";
            if (!string.IsNullOrWhiteSpace(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
            {
                var customers = await _customerService.GetAllAsyncWithUser();
                var customer = customers?.FirstOrDefault(c => c.UserId == userId);
                if (customer != null)
                {
                    defaultPhone = customer.User?.PhoneNumber ?? "";
                    defaultAddress = customer.Address ?? "";
                    defaultName = customer.User?.Fullname ?? "";
                }
            }
            ViewBag.DefaultPhone = defaultPhone;
            ViewBag.DefaultAddress = defaultAddress;
            ViewBag.DefaultName = defaultName;

            // Eager-load ProductDetail and lookups so selected items show full info
            var cartItems = await _cartDetailService.FindWithIncludesAsync(
                cd => cd.CartId == cartId,
                cd => cd.ProductDetail!,
                cd => cd.ProductDetail!.Product!,
                cd => cd.ProductDetail!.Length!,
                cd => cd.ProductDetail!.Hardness!,
                cd => cd.ProductDetail!.Color!
            );
            var selectedItems = (cartItems ?? Enumerable.Empty<CartDetail>())
                .Where(ci => ids.Contains(ci.ProductDetailId) && ci.Quantity > 0)
                .ToList();
            if (!selectedItems.Any())
            {
                TempData["Error"] = "Sản phẩm chọn không hợp lí";
                return Redirect($"/CartDetail?cartId={cartId}");
            }

            var pm = await _paymentMethodRepo.GetAllAsync();
            // Chỉ lấy phương thức thanh toán dành cho Online (COD)
            var onlinePaymentMethods = (pm ?? new List<PaymentMethod>())
                .Where(p => p.IsAvailableForOnline())
                .ToList();
            ViewBag.PaymentMethods = onlinePaymentMethods;
            ViewBag.CartId = cartId;
            ViewBag.SelectedIds = string.Join(',', selectedItems.Select(s => s.ProductDetailId));

            // Tính toán subtotal
            decimal subtotal = 0m;
            var vm = new List<object>();
            foreach (var ci in selectedItems)
            {
                var pd = ci.ProductDetail ?? await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                if (pd == null) continue;
                subtotal += pd.Price * ci.Quantity;
                vm.Add(new
                {
                    ProductDetailId = ci.ProductDetailId,
                    Name = pd.Product?.Name ?? "Sản phẩm",
                    Price = pd.Price,
                    Quantity = ci.Quantity,
                    Length = pd.Length?.Name,
                    Hardness = pd.Hardness?.Name
                });
            }

            // Lấy danh sách vouchers có thể sử dụng
            var allVouchers = await _voucherRepo.GetAllAsync();
            var availableVouchers = (allVouchers ?? new List<Voucher>())
                .Where(v => v.IsValid() && (!v.MinOrderAmount.HasValue || subtotal >= v.MinOrderAmount.Value))
                .OrderByDescending(v => v.DiscountPercentage)
                .ToList();
            ViewBag.AvailableVouchers = availableVouchers;
            ViewBag.Subtotal = subtotal;

            // Tính toán voucher discount từ session
            decimal voucherDiscount = 0m;
            var appliedCode = HttpContext.Session.GetString("SESSION_VOUCHER_CODE");
            if (!string.IsNullOrWhiteSpace(appliedCode))
            {
                var found = await _voucherRepo.FindAsync(v => v.Code == appliedCode);
                var voucher = found?.FirstOrDefault();
                if (voucher != null && voucher.IsValid())
                {
                    voucherDiscount = voucher.CalculateDiscount(subtotal);
                }
                else
                {
                    // Voucher không hợp lệ, xóa khỏi session
                    HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                    HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
                    appliedCode = null;
                }
            }

            var shippingFee = 35000m;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.VoucherDiscount = voucherDiscount;
            ViewBag.Total = subtotal + shippingFee - voucherDiscount;
            ViewBag.AppliedVoucherCode = appliedCode;

            return View(vm);
        }

        // POST: /Checkout/ApplyVoucher - Áp dụng mã giảm giá tại trang thanh toán
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyVoucher(Guid cartId, string selectedIds, string code)
        {
            code = code?.Trim().ToUpperInvariant() ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(code))
            {
                TempData["Error"] = "Vui lòng nhập mã voucher";
                return Redirect($"/Checkout/Start?cartId={cartId}&selected={selectedIds}");
            }

            // Tính subtotal để validate voucher
            var ids = (selectedIds ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Guid.TryParse(s, out var g) ? g : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToArray();

            var cartItems = await _cartDetailService.FindWithIncludesAsync(
                cd => cd.CartId == cartId,
                cd => cd.ProductDetail!
            );
            var selectedItems = (cartItems ?? Enumerable.Empty<CartDetail>())
                .Where(ci => ids.Contains(ci.ProductDetailId) && ci.Quantity > 0)
                .ToList();

            decimal subtotal = 0m;
            foreach (var ci in selectedItems)
            {
                var pd = ci.ProductDetail ?? await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                var price = pd?.Price ?? 0m;
                subtotal += price * ci.Quantity;
            }

            // Tìm voucher
            var found = await _voucherRepo.FindAsync(v => v.Code == code);
            var voucher = found?.FirstOrDefault();
            
            if (voucher == null)
            {
                TempData["Error"] = "Mã voucher không tồn tại";
                HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
                return Redirect($"/Checkout/Start?cartId={cartId}&selected={selectedIds}");
            }

            if (!voucher.IsValid())
            {
                if (voucher.EndDate.HasValue && voucher.EndDate.Value < DateTime.Now)
                {
                    TempData["Error"] = "Voucher đã hết hạn";
                }
                else if (voucher.StartDate.HasValue && voucher.StartDate.Value > DateTime.Now)
                {
                    TempData["Error"] = "Voucher chưa bắt đầu";
                }
                else if (voucher.MaxUsage.HasValue && voucher.UsageCount >= voucher.MaxUsage.Value)
                {
                    TempData["Error"] = "Voucher đã hết lượt sử dụng";
                }
                else
                {
                    TempData["Error"] = "Voucher không hợp lệ";
                }
                HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
                return Redirect($"/Checkout/Start?cartId={cartId}&selected={selectedIds}");
            }

            if (voucher.MinOrderAmount.HasValue && subtotal < voucher.MinOrderAmount.Value)
            {
                TempData["Error"] = $"Giá trị đơn hàng tối thiểu để áp dụng voucher là {voucher.MinOrderAmount.Value:#,##0}";
                HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
                return Redirect($"/Checkout/Start?cartId={cartId}&selected={selectedIds}");
            }

            var discount = voucher.CalculateDiscount(subtotal);
            HttpContext.Session.SetString("SESSION_VOUCHER_CODE", voucher.Code);
            HttpContext.Session.SetString("SESSION_VOUCHER_DISCOUNT", discount.ToString());
            TempData["Success"] = $"Áp dụng voucher '{voucher.Code}' thành công. Giảm {discount:#,##0}.";

            return Redirect($"/Checkout/Start?cartId={cartId}&selected={selectedIds}");
        }

        // POST: /Checkout/RemoveVoucher - Bỏ mã giảm giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveVoucher(Guid cartId, string selectedIds)
        {
            HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
            HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
            TempData["Success"] = "Đã bỏ voucher";
            return Redirect($"/Checkout/Start?cartId={cartId}&selected={selectedIds}");
        }

        // POST: /Checkout/ApplyVoucherAjax - Áp dụng mã giảm giá qua AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyVoucherAjax([FromBody] ApplyVoucherRequest request)
        {
            var code = request.Code?.Trim().ToUpperInvariant() ?? string.Empty;
            var cartId = request.CartId;
            var selectedIds = request.SelectedIds;

            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new { success = false, message = "Vui lòng nhập mã voucher" });
            }

            // Tính subtotal để validate voucher
            var ids = (selectedIds ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Guid.TryParse(s, out var g) ? g : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToArray();

            var cartItems = await _cartDetailService.FindWithIncludesAsync(
                cd => cd.CartId == cartId,
                cd => cd.ProductDetail!
            );
            var selectedItems = (cartItems ?? Enumerable.Empty<CartDetail>())
                .Where(ci => ids.Contains(ci.ProductDetailId) && ci.Quantity > 0)
                .ToList();

            decimal subtotal = 0m;
            foreach (var ci in selectedItems)
            {
                var pd = ci.ProductDetail ?? await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                var price = pd?.Price ?? 0m;
                subtotal += price * ci.Quantity;
            }

            // Tìm voucher
            var found = await _voucherRepo.FindAsync(v => v.Code == code);
            var voucher = found?.FirstOrDefault();

            if (voucher == null)
            {
                HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
                return Json(new { success = false, message = "Mã voucher không tồn tại" });
            }

            if (!voucher.IsValid())
            {
                string errorMsg;
                if (voucher.EndDate.HasValue && voucher.EndDate.Value < DateTime.Now)
                    errorMsg = "Voucher đã hết hạn";
                else if (voucher.StartDate.HasValue && voucher.StartDate.Value > DateTime.Now)
                    errorMsg = "Voucher chưa bắt đầu";
                else if (voucher.MaxUsage.HasValue && voucher.UsageCount >= voucher.MaxUsage.Value)
                    errorMsg = "Voucher đã hết lượt sử dụng";
                else
                    errorMsg = "Voucher không hợp lệ";

                HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
                return Json(new { success = false, message = errorMsg });
            }

            if (voucher.MinOrderAmount.HasValue && subtotal < voucher.MinOrderAmount.Value)
            {
                HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
                return Json(new { success = false, message = $"Giá trị đơn hàng tối thiểu để áp dụng voucher là {voucher.MinOrderAmount.Value:#,##0}đ" });
            }

            var discount = voucher.CalculateDiscount(subtotal);
            var shippingFee = 35000m;
            var total = subtotal + shippingFee - discount;

            HttpContext.Session.SetString("SESSION_VOUCHER_CODE", voucher.Code);
            HttpContext.Session.SetString("SESSION_VOUCHER_DISCOUNT", discount.ToString());

            // Lấy danh sách vouchers có thể sử dụng
            var allVouchers = await _voucherRepo.GetAllAsync();
            var availableVouchers = (allVouchers ?? new List<Voucher>())
                .Where(v => v.IsValid() && (!v.MinOrderAmount.HasValue || subtotal >= v.MinOrderAmount.Value))
                .OrderByDescending(v => v.DiscountPercentage)
                .Select(v => new
                {
                    code = v.Code,
                    percent = v.DiscountPercentage?.ToString("0") ?? "0",
                    max = v.MaxDiscountAmount.HasValue ? v.MaxDiscountAmount.Value.ToString("#,##0") + "đ" : "Không giới hạn",
                    min = v.MinOrderAmount.HasValue ? v.MinOrderAmount.Value.ToString("#,##0") + "đ" : "0đ",
                    hsd = v.EndDate.HasValue ? v.EndDate.Value.ToString("dd/MM/yyyy") : "Không xác định"
                })
                .ToList();

            return Json(new
            {
                success = true,
                message = $"Áp dụng voucher '{voucher.Code}' thành công. Giảm {discount:#,##0}đ.",
                appliedCode = voucher.Code,
                discount = discount,
                discountFormatted = discount.ToString("#,##0"),
                total = total,
                totalFormatted = total.ToString("#,##0"),
                subtotal = subtotal,
                subtotalFormatted = subtotal.ToString("#,##0"),
                availableVouchers = availableVouchers
            });
        }

        // POST: /Checkout/RemoveVoucherAjax - Bỏ mã giảm giá qua AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveVoucherAjax([FromBody] RemoveVoucherRequest request)
        {
            HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
            HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");

            var cartId = request.CartId;
            var selectedIds = request.SelectedIds;

            // Tính subtotal
            var ids = (selectedIds ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Guid.TryParse(s, out var g) ? g : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToArray();

            var cartItems = await _cartDetailService.FindWithIncludesAsync(
                cd => cd.CartId == cartId,
                cd => cd.ProductDetail!
            );
            var selectedItems = (cartItems ?? Enumerable.Empty<CartDetail>())
                .Where(ci => ids.Contains(ci.ProductDetailId) && ci.Quantity > 0)
                .ToList();

            decimal subtotal = 0m;
            foreach (var ci in selectedItems)
            {
                var pd = ci.ProductDetail ?? await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                var price = pd?.Price ?? 0m;
                subtotal += price * ci.Quantity;
            }

            var shippingFee = 35000m;
            var total = subtotal + shippingFee;

            // Lấy danh sách vouchers có thể sử dụng
            var allVouchers = await _voucherRepo.GetAllAsync();
            var availableVouchers = (allVouchers ?? new List<Voucher>())
                .Where(v => v.IsValid() && (!v.MinOrderAmount.HasValue || subtotal >= v.MinOrderAmount.Value))
                .OrderByDescending(v => v.DiscountPercentage)
                .Select(v => new
                {
                    code = v.Code,
                    percent = v.DiscountPercentage?.ToString("0") ?? "0",
                    max = v.MaxDiscountAmount.HasValue ? v.MaxDiscountAmount.Value.ToString("#,##0") + "đ" : "Không giới hạn",
                    min = v.MinOrderAmount.HasValue ? v.MinOrderAmount.Value.ToString("#,##0") + "đ" : "0đ",
                    hsd = v.EndDate.HasValue ? v.EndDate.Value.ToString("dd/MM/yyyy") : "Không xác định"
                })
                .ToList();

            return Json(new
            {
                success = true,
                message = "Đã bỏ voucher",
                discount = 0m,
                discountFormatted = "0",
                total = total,
                totalFormatted = total.ToString("#,##0"),
                subtotal = subtotal,
                subtotalFormatted = subtotal.ToString("#,##0"),
                availableVouchers = availableVouchers
            });
        }

        public class ApplyVoucherRequest
        {
            public Guid CartId { get; set; }
            public string? SelectedIds { get; set; }
            public string? Code { get; set; }
        }

        public class RemoveVoucherRequest
        {
            public Guid CartId { get; set; }
            public string? SelectedIds { get; set; }
        }

        // POST: /Checkout/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Guid cartId, string selectedIds, Guid paymentMethodId, string receiverName, string phoneNumber, string shippingAddress)
        {
            if (cartId == Guid.Empty) return BadRequest();
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) return Forbid();

            var customers = await _customerService.FindAsync(c => c.UserId == userId);
            var customer = customers?.FirstOrDefault();
            if (customer == null) return Forbid();

            var ids = (selectedIds ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Guid.TryParse(s, out var g) ? g : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToArray();
            if (ids.Length == 0) return BadRequest("Không có sản phẩm chọn");

            var cartItems = await _cartDetailService.FindWithIncludesAsync(
                cd => cd.CartId == cartId,
                cd => cd.ProductDetail!,
                cd => cd.ProductDetail!.Product!,
                cd => cd.ProductDetail!.Length!,
                cd => cd.ProductDetail!.Hardness!,
                cd => cd.ProductDetail!.Color!
            );
            var selectedItems = (cartItems ?? Enumerable.Empty<CartDetail>())
                .Where(ci => ids.Contains(ci.ProductDetailId) && ci.Quantity > 0)
                .ToList();
            if (!selectedItems.Any()) return BadRequest("Không có sản phẩm hợp lí");

            // Kiểm tra số lượng tồn kho trước khi đặt hàng
            var stockErrors = new List<string>();
            foreach (var ci in selectedItems)
            {
                var pd = ci.ProductDetail ?? await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                if (pd == null) continue;
                if (ci.Quantity > pd.StockQuantity)
                {
                    var productName = pd.Product?.Name ?? "Sản phẩm";
                    stockErrors.Add($"'{productName}' chỉ còn {pd.StockQuantity} sản phẩm trong kho (bạn yêu cầu {ci.Quantity})");
                }
            }
            if (stockErrors.Any())
            {
                TempData["Error"] = "Một số sản phẩm không đủ số lượng: " + string.Join("; ", stockErrors);
                var sel = string.Join(',', selectedItems.Select(s => s.ProductDetailId));
                return Redirect($"/Checkout/Start?cartId={cartId}&selected={sel}");
            }

            // Ensure we have up-to-date ProductDetail for price calculation
            decimal subtotal = 0m;
            foreach (var ci in selectedItems)
            {
                var pd = ci.ProductDetail ?? await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                var price = pd?.Price ?? 0m;
                subtotal += price * ci.Quantity;
            }

            // Apply shipping fee and voucher discount similar to CartDetailController.Index
            var shippingFee = 35000m;
            decimal discount = 0m;
            Voucher? appliedVoucher = null;
            var appliedCode = HttpContext.Session.GetString("SESSION_VOUCHER_CODE");
            if (!string.IsNullOrWhiteSpace(appliedCode))
            {
                var found = await _voucherRepo.FindAsync(v => v.Code == appliedCode);
                var voucher = found?.FirstOrDefault();
                if (voucher != null && voucher.IsValid())
                {
                    discount = voucher.CalculateDiscount(subtotal);
                    if (discount > 0)
                    {
                        appliedVoucher = voucher;
                    }
                }
            }

            var total = subtotal; // TotalAmount = subtotal without shipping
            var final = subtotal + shippingFee - discount;

            Order order;
            try
            {
                order = Order.Create(customer.Id, paymentMethodId, total, final, phoneNumber, shippingAddress);
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                var sel = string.Join(',', selectedItems.Select(s => s.ProductDetailId));
                return Redirect($"/Checkout/Start?cartId={cartId}&selected={sel}");
            }
            order.Status = "0";
            order.DiscountAmount = discount;
            order.ReceiverName = receiverName?.Trim(); // Lưu tên người nhận

            // Nếu có voucher được áp dụng, tăng UsageCount và lưu VoucherId
            if (appliedVoucher != null)
            {
                order.VoucherId = appliedVoucher.Id;
                appliedVoucher.IncrementUsage();
                await _voucherRepo.UpdateAsync(appliedVoucher);
                await _voucherRepo.SaveChangesAsync();
                
                // Xóa voucher khỏi session sau khi đã sử dụng
                HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                HttpContext.Session.Remove("SESSION_VOUCHER_DISCOUNT");
            }

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();

            foreach (var ci in selectedItems)
            {
                var pd = ci.ProductDetail ?? await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                if (pd == null) continue;
                var od = OrderDetail.Create(order.Id, ci.ProductDetailId, ci.Quantity, pd.Price);
                od.NameAtOrder = pd.Product?.Name;
                od.LengthAtOrder = pd.Length?.Name;
                od.HardnessAtOrder = pd.Hardness?.Name;
                od.ColorAtOrder = pd.Color?.Name;
                await _orderDetailRepo.AddAsync(od);
            }
            await _orderDetailRepo.SaveChangesAsync();

            foreach (var ci in selectedItems)
            {
                ci.Quantity = 0;
                await _cartDetailService.UpdateAsync(ci);
            }
            await _cartDetailService.SaveChangesAsync();

            return RedirectToAction("Review", "Orders", new { id = order.Id });
        }
    }
}
