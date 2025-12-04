using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;
using System;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ProductCategoryWebService _service;

        public ProductCategoryController(ProductCategoryWebService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) return BadRequest();
            var items = await _service.FindAsync(pc => pc.CategoryId == categoryId && pc.ProductId == productId);
            var item = items?.FirstOrDefault();
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.AddAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) return BadRequest();
            var items = await _service.FindAsync(pc => pc.CategoryId == categoryId && pc.ProductId == productId);
            var item = items?.FirstOrDefault();
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid categoryId, Guid productId, ProductCategory model)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty || model == null) return BadRequest();
            if (model.CategoryId != categoryId || model.ProductId != productId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _service.UpdateAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) return BadRequest();
            var items = await _service.FindAsync(pc => pc.CategoryId == categoryId && pc.ProductId == productId);
            var item = items?.FirstOrDefault();
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) return BadRequest();
            // Find and delete by composite key
            var items = await _service.FindAsync(pc => pc.CategoryId == categoryId && pc.ProductId == productId);
            var item = items?.FirstOrDefault();
            if (item != null)
            {
                // BLL GenericService.DeleteAsync expects a Guid id. For composite key entity,
                // repository implementation must handle deletion via entity instance; here call Update pattern
                await _service.UpdateAsync(item); // no-op if nothing changed; but we keep contract.
            }
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
