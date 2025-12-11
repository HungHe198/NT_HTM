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
        private readonly NT.BLL.Interfaces.IGenericRepository<Voucher> _voucherRepository;

        public CartController(CartWebService service, CartDetailWebService cartDetailService, ProductDetailWebService productDetailService, NT.BLL.Interfaces.IGenericRepository<Voucher> voucherRepository)
        {
            _service = service;
            _cartDetailService = cartDetailService;
            _productDetailService = productDetailService;
            _voucherRepository = voucherRepository;
        }

        // Session helpers
        private const string SessionCartKey = "SESSION_CART_ITEMS";
        private const string SessionVoucherKey = "SESSION_VOUCHER_CODE";
        private const string SessionVoucherDiscountKey = "SESSION_VOUCHER_DISCOUNT";
        private List<CartItemDto> GetSessionCart()
        {
            var json = HttpContext.Session.GetString(SessionCartKey);
            if (string.IsNullOrEmpty(json)) return new List<CartItemDto>();
            try { return JsonSerializer.Deserialize<List<CartItemDto>>(json) ?? new List<CartItemDto>(); }
            catch { return new List<CartItemDto>(); }
        }
        private void SaveSessionCart(List<CartItemDto> items)
        {
            var json = JsonSerializer.Serialize(items);
            HttpContext.Session.SetString(SessionCartKey, json);
        }

        public async Task<IActionResult> Index()
        {
            // Show session cart
            var items = GetSessionCart();
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
                if (voucher != null && (!voucher.ExpiryDate.HasValue || voucher.ExpiryDate.Value >= DateTime.UtcNow))
                {
                    if (!voucher.MinOrderAmount.HasValue || subtotal >= voucher.MinOrderAmount.Value)
                    {
                        appliedDiscount = voucher.DiscountAmount.GetValueOrDefault(0m);
                        if (voucher.MaxDiscountAmount.HasValue)
                            appliedDiscount = Math.Min(appliedDiscount, voucher.MaxDiscountAmount.Value);
                        appliedDiscount = Math.Min(appliedDiscount, subtotal);
                    }
                    else
                    {
                        // below minimum, ignore discount and show message
                        TempData["Error"] = $"Giá trị đơn hàng tối thiểu để áp dụng voucher là {voucher.MinOrderAmount.Value:#,##0}";
                        appliedDiscount = 0m;
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
            var detail = await _productDetailService.GetByIdAsync(productDetailId);
            if (detail is null) return NotFound();

            var items = GetSessionCart();
            var existing = items.FirstOrDefault(i => i.ProductDetailId == productDetailId);
            if (existing is null)
            {
                items.Add(new CartItemDto
                {
                    ProductDetailId = productDetailId,
                    ProductName = detail.Product?.Name ?? "Sản phẩm",
                    Thumbnail = detail.Product?.Thumbnail,
                    LengthName = detail.Length?.Name,
                    HardnessName = detail.Hardness?.Name,
                    UnitPrice = detail.Price,
                    Quantity = quantity
                });
            }
            else
            {
                existing.Quantity += quantity;
            }

            SaveSessionCart(items);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateQty(Guid productDetailId, int quantity)
        {
            var items = GetSessionCart();
            var item = items.FirstOrDefault(i => i.ProductDetailId == productDetailId);
            if (item is null) return NotFound();
            item.Quantity = Math.Max(1, quantity);
            SaveSessionCart(items);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(Guid productDetailId)
        {
            var items = GetSessionCart();
            items = items.Where(i => i.ProductDetailId != productDetailId).ToList();
            SaveSessionCart(items);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ApplyVoucher(string code)
        {
            code = code?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(code))
            {
                TempData["Error"] = "Vui lòng nhập mã voucher";
                return RedirectToAction(nameof(Index));
            }

            var items = GetSessionCart();
            var subtotal = items.Sum(i => i.UnitPrice * i.Quantity);

            var found = await _voucherRepository.FindAsync(v => v.Code == code);
            var voucher = found?.FirstOrDefault();
            if (voucher == null)
            {
                TempData["Error"] = "Mã voucher không hợp lệ";
                HttpContext.Session.Remove(SessionVoucherKey);
                HttpContext.Session.Remove(SessionVoucherDiscountKey);
                return RedirectToAction(nameof(Index));
            }

            if (voucher.ExpiryDate.HasValue && voucher.ExpiryDate.Value < DateTime.UtcNow)
            {
                TempData["Error"] = "Voucher đã hết hạn";
                HttpContext.Session.Remove(SessionVoucherKey);
                HttpContext.Session.Remove(SessionVoucherDiscountKey);
                return RedirectToAction(nameof(Index));
            }

            if (voucher.MaxUsage.HasValue && voucher.UsageCount.HasValue && voucher.UsageCount.Value >= voucher.MaxUsage.Value)
            {
                TempData["Error"] = "Voucher đã sử dụng hết";
                HttpContext.Session.Remove(SessionVoucherKey);
                HttpContext.Session.Remove(SessionVoucherDiscountKey);
                return RedirectToAction(nameof(Index));
            }

            if (voucher.MinOrderAmount.HasValue && subtotal < voucher.MinOrderAmount.Value)
            {
                TempData["Error"] = $"Giá trị đơn hàng tối thiểu để áp dụng voucher là {voucher.MinOrderAmount.Value:#,##0}";
                HttpContext.Session.Remove(SessionVoucherKey);
                HttpContext.Session.Remove(SessionVoucherDiscountKey);
                return RedirectToAction(nameof(Index));
            }

            var discount = voucher.DiscountAmount.GetValueOrDefault(0m);
            if (voucher.MaxDiscountAmount.HasValue)
            {
                discount = Math.Min(discount, voucher.MaxDiscountAmount.Value);
            }

            // Prevent discount exceeding subtotal
            discount = Math.Min(discount, subtotal);

            HttpContext.Session.SetString(SessionVoucherKey, voucher.Code);
            HttpContext.Session.SetString(SessionVoucherDiscountKey, discount.ToString());
            TempData["Success"] = $"Áp dụng voucher '{voucher.Code}' thành công. Giảm {discount:#,##0}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult RemoveVoucher()
        {
            HttpContext.Session.Remove(SessionVoucherKey);
            HttpContext.Session.Remove(SessionVoucherDiscountKey);
            TempData["Success"] = "Đã bỏ voucher";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View();

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
