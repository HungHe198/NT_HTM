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

        public CartController(CartWebService service, CartDetailWebService cartDetailService, ProductDetailWebService productDetailService)
        {
            _service = service;
            _cartDetailService = cartDetailService;
            _productDetailService = productDetailService;
        }

        // Session helpers
        private const string SessionCartKey = "SESSION_CART_ITEMS";
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
            ViewBag.Total = subtotal + ViewBag.ShippingFee;
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
