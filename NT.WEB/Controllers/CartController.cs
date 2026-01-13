using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NT.WEB.DTO;
using Microsoft.AspNetCore.Authorization;

namespace NT.WEB.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly CartWebService _service;
        private readonly CartDetailWebService _cartDetailService;
        private readonly ProductDetailWebService _productDetailService;
        private readonly CustomerWebService _customerService;
        private readonly NT.BLL.Interfaces.IGenericRepository<Voucher> _voucherRepository;

        public CartController(CartWebService service, CartDetailWebService cartDetailService, ProductDetailWebService productDetailService, CustomerWebService customerService, NT.BLL.Interfaces.IGenericRepository<Voucher> voucherRepository)
        {
            _service = service;
            _cartDetailService = cartDetailService;
            _productDetailService = productDetailService;
            _customerService = customerService;
            _voucherRepository = voucherRepository;
        }

        // Voucher session keys (voucher code stays in session, cart is persisted)
        private const string SessionVoucherKey = "SESSION_VOUCHER_CODE";
        private const string SessionVoucherDiscountKey = "SESSION_VOUCHER_DISCOUNT";

        // Helpers: resolve current customer's cart (create if missing)
        private async Task<Cart?> GetOrCreateCustomerCartAsync()
        {
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim)) return null;
            if (!Guid.TryParse(userIdClaim, out var userId)) return null;

            // Find customer by UserId
            var customers = await _customerService.FindAsync(c => c.UserId == userId);
            var customer = customers?.FirstOrDefault();
            if (customer == null) return null;

            // Find existing cart
            var carts = await _service.FindAsync(ct => ct.CustomerId == customer.Id);
            var cart = carts?.FirstOrDefault();
            if (cart != null) return cart;

            // Create new cart for customer
            var newCart = Cart.Create(customer.Id);
            await _service.AddAsync(newCart);
            await _service.SaveChangesAsync();
            return newCart;
        }

        public async Task<IActionResult> Index()
        {
            // Load cart items from database for current customer
            var cart = await GetOrCreateCustomerCartAsync();
            var items = new List<CartItemDto>();
            if (cart != null)
            {
                var cartItems = await _cartDetailService.FindAsync(cd => cd.CartId == cart.Id);
                foreach (var ci in cartItems ?? Enumerable.Empty<CartDetail>())
                {
                    var pd = ci.ProductDetail;
                    if (pd == null || ci.Quantity <= 0) continue;
                    items.Add(new CartItemDto
                    {
                        ProductDetailId = ci.ProductDetailId,
                        ProductCode = pd.Product?.ProductCode,
                        ProductName = pd.Product?.Name ?? "Sản phẩm",
                        Thumbnail = pd.Product?.Thumbnail,
                        LengthName = pd.Length?.Name,
                        HardnessName = pd.Hardness?.Name,
                        UnitPrice = pd.Price,
                        Quantity = ci.Quantity
                    });
                }
            }
            var subtotal = items.Sum(i => i.UnitPrice * i.Quantity);
            ViewBag.Subtotal = subtotal;
            ViewBag.ShippingFee = 35000m; // simple flat fee demo

            // Applied voucher (if any)
            var appliedCode = HttpContext.Session.GetString(SessionVoucherKey);
            decimal appliedDiscount = 0m;
            if (!string.IsNullOrWhiteSpace(appliedCode))
            {
                // re-validate voucher with current subtotal
                var found = await _voucherRepository.FindAsync(v => v.Code == appliedCode);
                var voucher = found?.FirstOrDefault();
                if (voucher != null && voucher.IsValid())
                {
                    appliedDiscount = voucher.CalculateDiscount(subtotal);
                    if (appliedDiscount == 0 && voucher.MinOrderAmount.HasValue && subtotal < voucher.MinOrderAmount.Value)
                    {
                        TempData["Error"] = $"Giá trị đơn hàng tối thiểu để áp dụng voucher là {voucher.MinOrderAmount.Value:#,##0}";
                    }
                }
                else
                {
                    // invalid voucher in session, clean up
                    HttpContext.Session.Remove(SessionVoucherKey);
                }
            }
            ViewBag.AppliedVoucherCode = appliedCode;
            ViewBag.VoucherDiscount = appliedDiscount;

            ViewBag.Total = subtotal + ViewBag.ShippingFee - appliedDiscount;
            return View(items);
        }

        // Add item from product detail selection
        [HttpPost]
        public async Task<IActionResult> Add(Guid productDetailId, int quantity)
        {
            if (productDetailId == Guid.Empty || quantity <= 0) return BadRequest();
            var cart = await GetOrCreateCustomerCartAsync();
            if (cart == null) return Forbid();
            var detail = await _productDetailService.GetByIdAsync(productDetailId);
            if (detail is null) return NotFound();
            
            // Kiểm tra sản phẩm có đang hoạt động không (Status = "1")
            // Nếu sản phẩm ngừng bán, không cho phép thêm vào giỏ hàng
            if (detail.Product != null && !NT.SHARED.Constants.ProductStatus.IsActive(detail.Product.Status) && !string.IsNullOrEmpty(detail.Product.Status))
            {
                TempData["Error"] = "Sản phẩm này đã ngừng bán và không thể thêm vào giỏ hàng.";
                return Redirect($"/CartDetail?cartId={cart.Id}");
            }

            // Kiểm tra số lượng tồn kho
            var existingDb = (await _cartDetailService.FindAsync(cd => cd.CartId == cart.Id && cd.ProductDetailId == productDetailId))?.FirstOrDefault();
            var currentQtyInCart = existingDb?.Quantity ?? 0;
            var totalRequestedQty = currentQtyInCart + quantity;

            if (totalRequestedQty > detail.StockQuantity)
            {
                TempData["Error"] = $"Số lượng yêu cầu ({totalRequestedQty}) vượt quá số lượng tồn kho ({detail.StockQuantity}). Hiện tại trong giỏ đã có {currentQtyInCart} sản phẩm.";
                return Redirect($"/CartDetail?cartId={cart.Id}");
            }

            // Persist to CartDetail
            if (existingDb == null)
            {
                var newItem = CartDetail.Create(cart.Id, productDetailId, quantity);
                await _cartDetailService.AddAsync(newItem);
            }
            else
            {
                existingDb.Quantity += quantity;
                await _cartDetailService.UpdateAsync(existingDb);
            }
            await _cartDetailService.SaveChangesAsync();
            // Redirect to CartDetail page with current cartId
            return Redirect($"/CartDetail?cartId={cart.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQty(Guid productDetailId, int quantity)
        {
            var cart = await GetOrCreateCustomerCartAsync();
            if (cart == null) return Forbid();
            var item = (await _cartDetailService.FindAsync(cd => cd.CartId == cart.Id && cd.ProductDetailId == productDetailId))?.FirstOrDefault();
            if (item is null) return NotFound();

            // Kiểm tra số lượng tồn kho
            var detail = await _productDetailService.GetByIdAsync(productDetailId);
            if (detail != null && quantity > detail.StockQuantity)
            {
                TempData["Error"] = $"Số lượng yêu cầu ({quantity}) vượt quá số lượng tồn kho ({detail.StockQuantity}).";
                return RedirectToAction(nameof(Index));
            }

            item.Quantity = Math.Max(1, quantity);
            await _cartDetailService.UpdateAsync(item);
            await _cartDetailService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid productDetailId)
        {
            var cart = await GetOrCreateCustomerCartAsync();
            if (cart == null) return Forbid();
            var item = (await _cartDetailService.FindAsync(cd => cd.CartId == cart.Id && cd.ProductDetailId == productDetailId))?.FirstOrDefault();
            if (item != null)
            {
                // Since GenericService.DeleteAsync expects Guid, and CartDetail has composite key,
                // update quantity to 0 as a soft-remove fallback or implement repository support.
                // Here we set quantity to 0 to exclude from totals.
                item.Quantity = 0;
                await _cartDetailService.UpdateAsync(item);
                await _cartDetailService.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ApplyVoucher(string code)
        {
            code = code?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(code))
            {
                TempData["Error"] = "Vui lòng nhập mã voucher";
                var cart = await GetOrCreateCustomerCartAsync();
                return Redirect($"/CartDetail?cartId={cart?.Id}");
            }
            var items = await GetCustomerCartItemsAsync();
            var subtotal = items.Sum(i => i.UnitPrice * i.Quantity);

            var found = await _voucherRepository.FindAsync(v => v.Code == code);
            var voucher = found?.FirstOrDefault();
            if (voucher == null)
            {
                TempData["Error"] = "Mã voucher không hợp lệ";
                HttpContext.Session.Remove(SessionVoucherKey);
                HttpContext.Session.Remove(SessionVoucherDiscountKey);
                var cart = await GetOrCreateCustomerCartAsync();
                return Redirect($"/CartDetail?cartId={cart?.Id}");
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
                    TempData["Error"] = "Voucher đã sử dụng hết";
                }
                else
                {
                    TempData["Error"] = "Voucher không hợp lệ";
                }
                HttpContext.Session.Remove(SessionVoucherKey);
                HttpContext.Session.Remove(SessionVoucherDiscountKey);
                var cart = await GetOrCreateCustomerCartAsync();
                return Redirect($"/CartDetail?cartId={cart?.Id}");
            }

            if (voucher.MinOrderAmount.HasValue && subtotal < voucher.MinOrderAmount.Value)
            {
                TempData["Error"] = $"Giá trị đơn hàng tối thiểu để áp dụng voucher là {voucher.MinOrderAmount.Value:#,##0}";
                HttpContext.Session.Remove(SessionVoucherKey);
                HttpContext.Session.Remove(SessionVoucherDiscountKey);
                var cart = await GetOrCreateCustomerCartAsync();
                return Redirect($"/CartDetail?cartId={cart?.Id}");
            }

            var discount = voucher.CalculateDiscount(subtotal);

            HttpContext.Session.SetString(SessionVoucherKey, voucher.Code);
            HttpContext.Session.SetString(SessionVoucherDiscountKey, discount.ToString());
            TempData["Success"] = $"Áp dụng voucher '{voucher.Code}' thành công. Giảm {discount:#,##0}.";
            var cartAfter = await GetOrCreateCustomerCartAsync();
            return Redirect($"/CartDetail?cartId={cartAfter?.Id}");
        }

        // Helper: load current customer's cart items from database
        private async Task<List<CartItemDto>> GetCustomerCartItemsAsync()
        {
            var cart = await GetOrCreateCustomerCartAsync();
            var items = new List<CartItemDto>();
            if (cart != null)
            {
                var cartItems = await _cartDetailService.FindAsync(cd => cd.CartId == cart.Id);
                foreach (var ci in cartItems ?? Enumerable.Empty<CartDetail>())
                {
                    var pd = ci.ProductDetail;
                    if (pd == null)
                    {
                        // Ensure we have ProductDetail loaded to get price when navigation is not populated
                        pd = await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                    }
                    if (pd == null || ci.Quantity <= 0) continue;
                    items.Add(new CartItemDto
                    {
                        ProductDetailId = ci.ProductDetailId,
                        ProductCode = pd.Product?.ProductCode,
                        ProductName = pd.Product?.Name ?? "Sản phẩm",
                        Thumbnail = pd.Product?.Thumbnail,
                        LengthName = pd.Length?.Name,
                        HardnessName = pd.Hardness?.Name,
                        UnitPrice = pd.Price,
                        Quantity = ci.Quantity
                    });
                }
            }
            return items;
        }

        [HttpPost]
        public IActionResult RemoveVoucher()
        {
            HttpContext.Session.Remove(SessionVoucherKey);
            HttpContext.Session.Remove(SessionVoucherDiscountKey);
            TempData["Success"] = "Đã bỏ voucher";
            return Redirect("/CartDetail");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View();

        // Create a new empty cart for the current customer (new invoice)
        [HttpPost]
        public async Task<IActionResult> NewCart()
        {
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim)) return Forbid();
            if (!Guid.TryParse(userIdClaim, out var userId)) return Forbid();

            var customers = await _customerService.FindAsync(c => c.UserId == userId);
            var customer = customers?.FirstOrDefault();
            if (customer == null) return Forbid();

            var newCart = Cart.Create(customer.Id);
            await _service.AddAsync(newCart);
            await _service.SaveChangesAsync();

            return Redirect($"/CartDetail?cartId={newCart.Id}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cart model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.AddAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Cart model)
        {
            if (id == Guid.Empty || model == null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _service.UpdateAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            await _service.DeleteAsync(id);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

// simple VM for session cart
public class CartItemVm
{
    public Guid ProductDetailId { get; set; }
    public string? ProductName { get; set; }
    public string? Thumbnail { get; set; }
    public string? LengthName { get; set; }
    public string? HardnessName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
}
