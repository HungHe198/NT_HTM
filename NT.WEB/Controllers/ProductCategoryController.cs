using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NT.SHARED.Models;
using NT.WEB.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ProductCategoryWebService _service;
        private readonly ProductWebService _productService;
        private readonly CategoryWebService _categoryService;

        public ProductCategoryController(
            ProductCategoryWebService service,
            ProductWebService productService,
            CategoryWebService categoryService)
        {
            _service = service;
            _productService = productService;
            _categoryService = categoryService;
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

        public async Task<IActionResult> Create()
        {
            await PopulateSelectListsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return View(model);
            }

            // Server-side: ensure referenced product and category exist
            var product = await _productService.GetByIdAsync(model.ProductId);
            if (product is null)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Sản phẩm không tồn tại.");
            }

            var category = await _categoryService.GetByIdAsync(model.CategoryId);
            if (category is null)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Danh mục không tồn tại.");
            }

            // Prevent duplicate mapping
            var existing = (await _service.FindAsync(pc => pc.ProductId == model.ProductId && pc.CategoryId == model.CategoryId)).FirstOrDefault();
            if (existing != null)
            {
                ModelState.AddModelError(string.Empty, "Liên kết sản phẩm - danh mục đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return View(model);
            }

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
            // populate selects so UI can show names
            await PopulateSelectListsAsync();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid categoryId, Guid productId, ProductCategory model)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty || model == null) return BadRequest();
            if (model.CategoryId != categoryId || model.ProductId != productId) return BadRequest();
            // Validate referenced entities exist
            var product = await _productService.GetByIdAsync(model.ProductId);
            if (product is null)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Sản phẩm không tồn tại.");
            }

            var category = await _categoryService.GetByIdAsync(model.CategoryId);
            if (category is null)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Danh mục không tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return View(model);
            }
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
            // Use service helper to delete by composite key
            var deleted = await _service.DeleteByIdsAsync(categoryId, productId);
            if (!deleted) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateSelectListsAsync()
        {
            var products = (await _productService.GetAllAsync()).ToList();
            var categories = (await _categoryService.GetAllAsync()).ToList();

            ViewBag.Products = products.Select(p => new SelectListItem(p.Name ?? p.ProductCode ?? p.Id.ToString(), p.Id.ToString())).ToList();
            ViewBag.Categories = categories.Select(c => new SelectListItem(c.Name ?? c.Id.ToString(), c.Id.ToString())).ToList();
        }
    }
}
