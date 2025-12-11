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

        public CartDetailController(CartDetailWebService service, NT.BLL.Interfaces.IGenericRepository<Voucher> voucherRepository, ProductDetailWebService productDetailService)
        {
            _service = service;
            _voucherRepository = voucherRepository;
            _productDetailService = productDetailService;
        }

        public async Task<IActionResult> Index(Guid cartId)
        {
            if (cartId == Guid.Empty)
            {
                return NotFound();
            }

            // Build DTO items from CartDetail for provided cartId
            var cartItems = await _service.FindAsync(cd => cd.CartId == cartId);
            var items = new List<CartItemDto>();
            foreach (var ci in cartItems ?? new List<CartDetail>())
            {
                var pd = ci.ProductDetail;
                if (pd == null)
                {
                    pd = await _productDetailService.GetByIdAsync(ci.ProductDetailId);
                }
                if (pd == null || ci.Quantity <= 0) continue;
                items.Add(new CartItemDto
                {
                    ProductDetailId = ci.ProductDetailId,
                    ProductName = pd.Product?.Name ?? "S?n ph?m",
                    Thumbnail = pd.Product?.Thumbnail,
                    LengthName = pd.Length?.Name,
                    HardnessName = pd.Hardness?.Name,
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
                        TempData["Error"] = $"Giá tr? ??n hàng t?i thi?u ?? áp d?ng voucher là {voucher.MinOrderAmount.Value:#,##0}";
                        appliedDiscount = 0m;
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
