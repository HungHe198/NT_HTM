using System;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;

namespace NT.WEB.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductWebService _productService;
        private readonly ProductDetailWebService _productDetailService;

        public ProductController(ProductWebService productService, ProductDetailWebService productDetailService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _productDetailService = productDetailService ?? throw new ArgumentNullException(nameof(productDetailService));
        }

        // GET: /Product
        public async Task<IActionResult> Index(string? q)
        {
            var model = string.IsNullOrWhiteSpace(q)
                ? await _productService.GetAllAsync()
                : await _productService.SearchByNameAsync(q);
            return View(model);
        }

        // GET: /Product/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();

            var details = (await _productDetailService.GetByProductIdAsync(id)).ToList();
            ViewBag.ProductDetails = details;

            return View(product);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            await _productService.AddAsync(model);
            await _productService.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();

            return View(product);
        }

        // POST: /Product/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product model)
        {
            if (id == Guid.Empty || model is null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            await _productService.UpdateAsync(model);
            await _productService.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();

            return View(product);
        }

        // POST: /Product/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            await _productService.DeleteAsync(id);
            await _productService.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ---------------- ProductDetail CRUD (minimal) ----------------

        // GET: /Product/CreateDetail/{productId}
        public IActionResult CreateDetail(Guid productId)
        {
            if (productId == Guid.Empty) return BadRequest();
            var model = (ProductDetail?)null;
            ViewBag.ProductId = productId;
            return View("ProductDetailCreate", model);
        }

        // POST: /Product/CreateDetail/{productId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDetail(Guid productId, ProductDetail model)
        {
            if (productId == Guid.Empty || model is null) return BadRequest();
            if (!ModelState.IsValid) { ViewBag.ProductId = productId; return View("ProductDetailCreate", model); }

            // ensure FK
            var detail = ProductDetail.Create(productId, model.Length.Id, model.SurfaceFinish.Id, model.Hardness.Id, model.Elasticity.Id, model.OriginCountry.Id, model.Color.Id, model.Price, model.StockQuantity);
            await _productDetailService.AddAsync(detail);
            await _productDetailService.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = productId });
        }

        // GET: /Product/EditDetail/{id}
        public async Task<IActionResult> EditDetail(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var detail = await _productDetailService.GetByIdAsync(id);
            if (detail is null) return NotFound();
            return View("ProductDetailEdit", detail);
        }

        // POST: /Product/EditDetail/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetail(Guid id, ProductDetail model)
        {
            if (id == Guid.Empty || model is null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View("ProductDetailEdit", model);

            await _productDetailService.UpdateAsync(model);
            await _productDetailService.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = model.ProductId });
        }

        // POST: /Product/DeleteDetail/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDetail(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var detail = await _productDetailService.GetByIdAsync(id);
            if (detail is null) return NotFound();

            var productId = detail.ProductId;
            await _productDetailService.DeleteAsync(id);
            await _productDetailService.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = productId });
        }
    }
}
