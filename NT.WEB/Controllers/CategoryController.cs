using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;

namespace NT.WEB.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryWebService _service;

        public CategoryController(CategoryWebService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: /Category
        public async Task<IActionResult> Index(string? q)
        {
            var model = string.IsNullOrWhiteSpace(q)
                ? await _service.GetAllAsync()
                : await _service.SearchByNameAsync(q);

            return View(model);
        }

        // GET: /Category/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var category = await _service.GetByIdAsync(id);
            if (category is null) return NotFound();

            return View(category);
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            await _service.AddAsync(model);
            await _service.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var category = await _service.GetByIdAsync(id);
            if (category is null) return NotFound();

            return View(category);
        }

        // POST: /Category/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Category model)
        {
            if (id == Guid.Empty || model is null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            await _service.UpdateAsync(model);
            await _service.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var category = await _service.GetByIdAsync(id);
            if (category is null) return NotFound();

            return View(category);
        }

        // POST: /Category/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            await _service.DeleteAsync(id);
            await _service.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Optional: AJAX / partial search endpoint used by Razor Pages or client-side code
        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            var result = await _service.SearchByNameAsync(q);
            return Json(result);
        }
    }
}
