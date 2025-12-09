using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        // POST: /ProductImage/Upload
        // Uploads an image file to wwwroot/uploads/products and returns the public URL.
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest(new { error = "No file provided" });

            var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
            if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsRoot, fileName);

            try
            {
                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to save file", detail = ex.Message });
            }

            var publicUrl = $"/uploads/products/{fileName}";
            return Json(new { url = publicUrl });
        }
    }
}