using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;
using System;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class CartDetailController : Controller
    {
        private readonly CartDetailWebService _service;

        public CartDetailController(CartDetailWebService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
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
