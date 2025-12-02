using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;

namespace NT.WEB.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly ProductImageWebService _service;

        public ProductImageController(ProductImageWebService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: /ProductImage
        public async Task<IActionResult> Index(string? q)
        {
            var model = string.IsNullOrWhiteSpace(q)
                ? await _service.GetAllAsync()
                : await _service.SearchByUrlAsync(q);
            return View(model);
        }

        // GET: /ProductImage/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            return View(item);
        }

        // GET: /ProductImage/Create
        public IActionResult Create() => View();

        // POST: /ProductImage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductImage model)
        {
            if (!ModelState.IsValid) return View(model);

            await _service.AddAsync(model);
            await _service.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /ProductImage/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            return View(item);
        }

        // POST: /ProductImage/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductImage model)
        {
            if (id == Guid.Empty || model is null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            await _service.UpdateAsync(model);
            await _service.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /ProductImage/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();

            return View(item);
        }

        // POST: /ProductImage/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            await _service.DeleteAsync(id);
            await _service.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Optional AJAX search by productDetailId
        [HttpGet]
        public async Task<IActionResult> ByProductDetail(Guid productDetailId)
        {
            if (productDetailId == Guid.Empty) return BadRequest();
            var items = await _service.GetByProductDetailIdAsync(productDetailId);
            return Json(items);
        }
    }
}