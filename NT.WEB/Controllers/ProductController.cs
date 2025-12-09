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
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ProductController : Controller
    {
        private readonly ProductWebService _productService;
        private readonly ProductDetailWebService _productDetailService;
        private readonly BrandWebService _brandService;
        private readonly Microsoft.Extensions.Logging.ILogger<ProductController> _logger;

        public ProductController(ProductWebService productService, ProductDetailWebService productDetailService, BrandWebService brandService, Microsoft.Extensions.Logging.ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _productDetailService = productDetailService ?? throw new ArgumentNullException(nameof(productDetailService));
            _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        public async Task<IActionResult> Create()
        {
            // prepare brand dropdown
            ViewBag.BrandSelectList = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            _logger.LogInformation("Create POST invoked. Model values: BrandId={BrandId}, ProductCode={ProductCode}, Name={Name}", model?.BrandId, model?.ProductCode, model?.Name);
            try
            {
                if (Request.HasFormContentType)
                {
                    var formPairs = Request.Form.Select(kvp => kvp.Key + "=" + string.Join(",", kvp.Value.ToArray()));
                    _logger.LogDebug("Create POST form: {Form}", string.Join(";", formPairs));
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Failed to read request form");
            }
            // ensure Brand chosen
            if (model.BrandId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.BrandId), "Vui lòng chọn Thương hiệu.");
            }
            if (!ModelState.IsValid)
            {
                // Log ModelState errors to help debugging
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Create Product validation failed: {Errors}", errors);
                // repopulate brand list when redisplaying form
                ViewBag.BrandSelectList = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
                return View(model);
            }

            try
            {
                await _productService.AddAsync(model);
                await _productService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create product");
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tạo sản phẩm. Vui lòng thử lại hoặc kiểm tra log.");
                ViewBag.BrandSelectList = new SelectList(await _brandService.GetAllAsync(), "Id", "Name", model.BrandId);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();

            // prepare brand dropdown with selected value
            ViewBag.BrandSelectList = new SelectList(await _brandService.GetAllAsync(), "Id", "Name", product.BrandId);
            return View(product);
        }

        // POST: /Product/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product model)
        {
            if (id == Guid.Empty || model is null || id != model.Id) return BadRequest();

            // ensure Brand chosen
            if (model.BrandId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.BrandId), "Vui lòng chọn Thương hiệu.");
            }

            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Edit Product validation failed for {ProductId}: {Errors}", id, errors);
                // repopulate brand list when returning view on validation error
                ViewBag.BrandSelectList = new SelectList(await _brandService.GetAllAsync(), "Id", "Name", model.BrandId);
                return View(model);
            }

            try
            {
                await _productService.UpdateAsync(model);
                await _productService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update product {ProductId}", id);
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi cập nhật sản phẩm. Vui lòng thử lại hoặc kiểm tra log.");
                ViewBag.BrandSelectList = new SelectList(await _brandService.GetAllAsync(), "Id", "Name", model.BrandId);
                return View(model);
            }

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

        [HttpGet]
        public async Task<IActionResult> Suggest(string q)
        {
            var query = q ?? string.Empty;
            if (string.IsNullOrWhiteSpace(query)) return Json(Array.Empty<object>());

            var items = await _productService.SearchByNameAsync(query);
            var result = items.Select(p => new { p.Id, p.Name, p.ProductCode });
            return Json(result);
        }
    }
}
