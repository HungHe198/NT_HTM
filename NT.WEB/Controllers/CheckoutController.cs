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
                TempData["Error"] = "Vui lòng ch?n s?n ph?m ?? thanh toán";
                return Redirect($"/CartDetail?cartId={cartId}");
            }

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
                TempData["Error"] = "S?n ph?m ch?n không h?p l?";
                return Redirect($"/CartDetail?cartId={cartId}");
            }

            var pm = await _paymentMethodRepo.GetAllAsync();
            ViewBag.PaymentMethods = pm ?? new List<PaymentMethod>();
            ViewBag.CartId = cartId;
            ViewBag.SelectedIds = string.Join(',', selectedItems.Select(s => s.ProductDetailId));

            var vm = new List<object>();
            foreach (var ci in selectedItems)
            {
                var pd = ci.ProductDetail ?? await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                if (pd == null) continue;
                vm.Add(new
                {
                    ProductDetailId = ci.ProductDetailId,
                    Name = pd.Product?.Name ?? "S?n ph?m",
                    Price = pd.Price,
                    Quantity = ci.Quantity,
                    Length = pd.Length?.Name,
                    Hardness = pd.Hardness?.Name
                });
            }
            return View(vm);
        }

        // POST: /Checkout/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Guid cartId, string selectedIds, Guid paymentMethodId, string phoneNumber, string shippingAddress)
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
            if (ids.Length == 0) return BadRequest("Không có s?n ph?m ch?n");

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
            if (!selectedItems.Any()) return BadRequest("Không có s?n ph?m h?p l?");

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
            var appliedCode = HttpContext.Session.GetString("SESSION_VOUCHER_CODE");
            if (!string.IsNullOrWhiteSpace(appliedCode))
            {
                var found = await _voucherRepo.FindAsync(v => v.Code == appliedCode);
                var voucher = found?.FirstOrDefault();
                if (voucher != null && voucher.IsValid())
                {
                    discount = voucher.CalculateDiscount(subtotal);
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
