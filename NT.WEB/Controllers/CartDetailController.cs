using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;
using NT.WEB.DTO;
using System;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class CartDetailController : Controller
    {
        private readonly CartDetailWebService _service;
        private readonly NT.BLL.Interfaces.IGenericRepository<Voucher> _voucherRepository;
        private readonly ProductDetailWebService _productDetailService;
        private readonly CartWebService _cartService;
        private readonly CustomerWebService _customerService;

        public CartDetailController(
            CartDetailWebService service,
            NT.BLL.Interfaces.IGenericRepository<Voucher> voucherRepository,
            ProductDetailWebService productDetailService,
            CartWebService cartService,
            CustomerWebService customerService)
        {
            _service = service;
            _voucherRepository = voucherRepository;
            _productDetailService = productDetailService;
            _cartService = cartService;
            _customerService = customerService;
        }

        // Resolve current customer's cart (create if missing)
        private async Task<Cart?> GetOrCreateCustomerCartAsync()
        {
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return null;
            }

            var customers = await _customerService.FindAsync(c => c.UserId == userId);
            var customer = customers?.FirstOrDefault();
            if (customer == null)
            {
                return null;
            }

            var carts = await _cartService.FindAsync(ct => ct.CustomerId == customer.Id);
            var cart = carts?.FirstOrDefault();
            if (cart != null) return cart;

            var newCart = Cart.Create(customer.Id);
            await _cartService.AddAsync(newCart);
            await _cartService.SaveChangesAsync();
            return newCart;
        }

        public async Task<IActionResult> Index(Guid cartId)
        {
            if (cartId == Guid.Empty)
            {
                // Try to resolve current user's cart if not provided
                var cart = await GetOrCreateCustomerCartAsync();
                if (cart == null)
                {
                    return NotFound();
                }
                cartId = cart.Id;
            }

            // Build DTO items from CartDetail for provided cartId
            // Eager-load ProductDetail and related lookups so UI has full info
            var cartItems = await _service.FindWithIncludesAsync(
                cd => cd.CartId == cartId,
                cd => cd.ProductDetail!,
                cd => cd.ProductDetail!.Product!,
                cd => cd.ProductDetail!.Length!,
                cd => cd.ProductDetail!.Hardness!,
                cd => cd.ProductDetail!.Color!
            );
            var items = new List<CartItemDto>();
            foreach (var ci in cartItems ?? new List<CartDetail>())
            {
                var pd = ci.ProductDetail;
                if (pd == null)
                {
                    pd = await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                }
                if (pd == null) continue;
                items.Add(new CartItemDto
                {
                    ProductDetailId = ci.ProductDetailId,
                    ProductName = pd.Product?.Name ?? "S?n ph?m",
                    Thumbnail = pd.Product?.Thumbnail,
                    LengthName = pd.Length?.Name,
                    HardnessName = pd.Hardness?.Name,
                    ColorName = pd.Color?.Name,
                    UnitPrice = pd.Price,
                    Quantity = ci.Quantity
                });
            }

            var subtotal = items.Sum(i => i.UnitPrice * i.Quantity);
            ViewBag.Subtotal = subtotal;
            ViewBag.ShippingFee = 35000m;

            var appliedCode = HttpContext.Session.GetString("SESSION_VOUCHER_CODE");
            decimal appliedDiscount = 0m;
            if (!string.IsNullOrWhiteSpace(appliedCode))
            {
                var found = await _voucherRepository.FindAsync(v => v.Code == appliedCode);
                var voucher = found?.FirstOrDefault();
                if (voucher != null && voucher.IsValid())
                {
                    appliedDiscount = voucher.CalculateDiscount(subtotal);
                    if (appliedDiscount == 0 && voucher.MinOrderAmount.HasValue && subtotal < voucher.MinOrderAmount.Value)
                    {
                        TempData["Error"] = $"Giá tr? ??n hàng t?i thi?u ?? áp d?ng voucher là {voucher.MinOrderAmount.Value:#,##0}";
                    }
                }
                else
                {
                    HttpContext.Session.Remove("SESSION_VOUCHER_CODE");
                }
            }
            ViewBag.AppliedVoucherCode = appliedCode;
            ViewBag.VoucherDiscount = appliedDiscount;
            ViewBag.Total = subtotal + ViewBag.ShippingFee - appliedDiscount;
            ViewBag.CartId = cartId;

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQty(Guid cartId, Guid productDetailId, int quantity)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) return BadRequest();
            var item = (await _service.FindAsync(cd => cd.CartId == cartId && cd.ProductDetailId == productDetailId))?.FirstOrDefault();
            if (item is null) return NotFound();
            item.Quantity = Math.Max(1, quantity);
            await _service.UpdateAsync(item);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { cartId });
        }

        /// <summary>
        /// AJAX endpoint for updating quantity without page reload
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateQtyAjax(Guid cartId, Guid productDetailId, int quantity)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty)
            {
                return Json(new { success = false, message = "Invalid parameters" });
            }

            try
            {
                var item = (await _service.FindAsync(cd => cd.CartId == cartId && cd.ProductDetailId == productDetailId))?.FirstOrDefault();
                if (item is null)
                {
                    return Json(new { success = false, message = "Item not found" });
                }

                item.Quantity = Math.Max(1, quantity);
                await _service.UpdateAsync(item);
                await _service.SaveChangesAsync();

                // Get unit price for the product detail
                var productDetail = await _productDetailService.GetByIdAsync(productDetailId);
                var unitPrice = productDetail?.Price ?? 0;

                // Calculate total items in cart
                var allItems = await _service.FindAsync(cd => cd.CartId == cartId && cd.Quantity > 0);
                var totalItems = allItems?.Count() ?? 0;

                return Json(new
                {
                    success = true,
                    quantity = item.Quantity,
                    unitPrice = unitPrice,
                    subtotal = unitPrice * item.Quantity,
                    totalItems = totalItems
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid cartId, Guid productDetailId)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) return BadRequest();
            var item = (await _service.FindAsync(cd => cd.CartId == cartId && cd.ProductDetailId == productDetailId))?.FirstOrDefault();
            if (item != null)
            {
                item.Quantity = 0;
                await _service.UpdateAsync(item);
                await _service.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { cartId });
        }

        // Checkout only selected items in this cart
        [HttpPost]
        public async Task<IActionResult> CheckoutSelected(Guid cartId, [FromForm] Guid[] selectedIds)
        {
            if (cartId == Guid.Empty) return BadRequest();
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["Error"] = "Vui lòng ch?n ít nh?t m?t s?n ph?m ?? thanh toán.";
                return RedirectToAction(nameof(Index), new { cartId });
            }

            var cartItems = await _service.FindAsync(cd => cd.CartId == cartId);
            var selected = (cartItems ?? Enumerable.Empty<CartDetail>())
                .Where(ci => selectedIds.Contains(ci.ProductDetailId))
                .ToList();

            if (!selected.Any())
            {
                TempData["Error"] = "Không có s?n ph?m h?p l? ?? thanh toán.";
                return RedirectToAction(nameof(Index), new { cartId });
            }

            // Calculate subtotal for selected items
            decimal selectedSubtotal = 0m;
            foreach (var s in selected)
            {
                var pd = s.ProductDetail ?? await _productDetailService.GetByIdAsync(s.ProductDetailId);
                if (pd != null && s.Quantity > 0)
                {
                    selectedSubtotal += pd.Price * s.Quantity;
                }
            }

            // Read voucher from session and validate against selected subtotal
            var appliedCode = HttpContext.Session.GetString("SESSION_VOUCHER_CODE");
            decimal appliedDiscount = 0m;
            if (!string.IsNullOrWhiteSpace(appliedCode))
            {
                var found = await _voucherRepository.FindAsync(v => v.Code == appliedCode);
                var voucher = found?.FirstOrDefault();
                if (voucher != null && voucher.IsValid())
                {
                    appliedDiscount = voucher.CalculateDiscount(selectedSubtotal);
                }
            }

            var selectedQuery = string.Join(',', selected.Select(s => s.ProductDetailId));
            // pass voucher info and computed totals to checkout
            var shipping = 35000m;
            var totalAfterDiscount = selectedSubtotal + shipping - appliedDiscount;
            return Redirect($"/Checkout/Start?cartId={cartId}&selected={selectedQuery}&code={appliedCode}&discount={appliedDiscount}&subtotal={selectedSubtotal}&shipping={shipping}&total={totalAfterDiscount}");
        }

        public async Task<IActionResult> Details(Guid cartId, Guid productDetailId)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) return BadRequest();
            var items = await _service.FindAsync(cd => cd.CartId == cartId && cd.ProductDetailId == productDetailId);
            var item = items?.FirstOrDefault();
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CartDetail model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.AddAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid cartId, Guid productDetailId)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) return BadRequest();
            var items = await _service.FindAsync(cd => cd.CartId == cartId && cd.ProductDetailId == productDetailId);
            var item = items?.FirstOrDefault();
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid cartId, Guid productDetailId, CartDetail model)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty || model == null) return BadRequest();
            if (model.CartId != cartId || model.ProductDetailId != productDetailId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _service.UpdateAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid cartId, Guid productDetailId)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) return BadRequest();
            var items = await _service.FindAsync(cd => cd.CartId == cartId && cd.ProductDetailId == productDetailId);
            var item = items?.FirstOrDefault();
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid cartId, Guid productDetailId)
        {
            if (cartId == Guid.Empty || productDetailId == Guid.Empty) return BadRequest();
            var items = await _service.FindAsync(cd => cd.CartId == cartId && cd.ProductDetailId == productDetailId);
            var item = items?.FirstOrDefault();
            if (item != null)
            {
                await _service.UpdateAsync(item); // placeholder due to repository delete contract expecting Guid
            }
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
