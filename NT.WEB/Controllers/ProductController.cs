using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        private readonly LengthWebService _lengthService;
        private readonly SurfaceFinishWebService _surfaceFinishService;
        private readonly HardnessWebService _hardnessService;
        private readonly ElasticityWebService _elasticityService;
        private readonly OriginCountryWebService _originCountryService;
        private readonly ColorWebService _colorService;
        private readonly ProductImageWebService _productImageService;
        private readonly Microsoft.Extensions.Logging.ILogger<ProductController> _logger;

        public ProductController(ProductWebService productService,
                                 ProductDetailWebService productDetailService,
                                 BrandWebService brandService,
                                 LengthWebService lengthService,
                                 SurfaceFinishWebService surfaceFinishService,
                                 HardnessWebService hardnessService,
                                 ElasticityWebService elasticityService,
                                 OriginCountryWebService originCountryService,
                                 ColorWebService colorService,
                                 ProductImageWebService productImageService,
                                 Microsoft.Extensions.Logging.ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _productDetailService = productDetailService ?? throw new ArgumentNullException(nameof(productDetailService));
            _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
            _lengthService = lengthService ?? throw new ArgumentNullException(nameof(lengthService));
            _surfaceFinishService = surfaceFinishService ?? throw new ArgumentNullException(nameof(surfaceFinishService));
            _hardnessService = hardnessService ?? throw new ArgumentNullException(nameof(hardnessService));
            _elasticityService = elasticityService ?? throw new ArgumentNullException(nameof(elasticityService));
            _originCountryService = originCountryService ?? throw new ArgumentNullException(nameof(originCountryService));
            _colorService = colorService ?? throw new ArgumentNullException(nameof(colorService));
            _productImageService = productImageService ?? throw new ArgumentNullException(nameof(productImageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: /Product
        public async Task<IActionResult> Index(string? q)
        {
            var model = string.IsNullOrWhiteSpace(q)
                ? await _productService.GetAllAsync()
                : await _productService.SearchByNameAsync(q);

            // ensure product has a thumbnail: pick first image from first active detail
            foreach (var p in model)
            {
                if (!string.IsNullOrWhiteSpace(p.Thumbnail)) continue;
                try
                {
                    var details = await _productDetailService.GetWithLookupsByProductIdAsync(p.Id);
                    var firstDetail = details.FirstOrDefault();
                    var firstImg = firstDetail?.Images?.FirstOrDefault();
                    if (firstImg != null && !string.IsNullOrWhiteSpace(firstImg.ImageUrl))
                    {
                        p.Thumbnail = firstImg.ImageUrl;
                    }
                }
                catch { }
            }
            return View(model);
        }

        // GET: /Product/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();

            var details = (await _productDetailService.GetWithLookupsByProductIdAsync(id)).ToList();
            ViewBag.ProductDetails = details;

            return View(product);
        }

        // GET: /Product/ProductDetailIndex/{id}
        public async Task<IActionResult> ProductDetailIndex(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();

            var details = (await _productDetailService.GetWithLookupsByProductIdAsync(id)).ToList();
            var hardnessIds = details.Select(d => d.HardnessId).Distinct().ToList();
            var lengthIds = details.Select(d => d.LengthId).Distinct().ToList();

            ViewBag.Product = product;
            ViewBag.Details = details;
            ViewBag.HardnessOptions = (await _hardnessService.GetAllAsync()).Where(h => hardnessIds.Contains(h.Id)).OrderBy(h => h.Name).ToList();
            ViewBag.LengthOptions = (await _lengthService.GetAllAsync()).Where(l => lengthIds.Contains(l.Id)).OrderBy(l => l.Name).ToList();

            return View("ProductDetailIndex");
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
            //if (!ModelState.IsValid)
            //{
            //    // Log ModelState errors to help debugging
            //    var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            //    _logger.LogWarning("Create Product validation failed: {Errors}", errors);
            //    // repopulate brand list when redisplaying form
            //    ViewBag.BrandSelectList = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            //    return View(model);
            //}

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
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi cập nhật sản phẩm. Vui lòng thử lại hoặc kiểm tra đăng nhập.");
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
            // populate lookup select lists
            ViewBag.LengthSelectList = new SelectList(_lengthService.GetAllAsync().Result, "Id", "Name");
            ViewBag.SurfaceFinishSelectList = new SelectList(_surfaceFinishService.GetAllAsync().Result, "Id", "Name");
            ViewBag.HardnessSelectList = new SelectList(_hardnessService.GetAllAsync().Result, "Id", "Name");
            ViewBag.ElasticitySelectList = new SelectList(_elasticityService.GetAllAsync().Result, "Id", "Name");
            ViewBag.OriginCountrySelectList = new SelectList(_originCountryService.GetAllAsync().Result, "Id", "Name");
            ViewBag.ColorSelectList = new SelectList(_colorService.GetAllAsync().Result, "Id", "Name");

            return View("ProductDetailCreate", model);
        }

        // POST: /Product/CreateDetail/{productId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDetail(Guid productId, ProductDetail model)
        {
            if (productId == Guid.Empty || model is null) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = productId;
                ViewBag.LengthSelectList = new SelectList(await _lengthService.GetAllAsync(), "Id", "Name");
                ViewBag.SurfaceFinishSelectList = new SelectList(await _surfaceFinishService.GetAllAsync(), "Id", "Name");
                ViewBag.HardnessSelectList = new SelectList(await _hardnessService.GetAllAsync(), "Id", "Name");
                ViewBag.ElasticitySelectList = new SelectList(await _elasticityService.GetAllAsync(), "Id", "Name");
                ViewBag.OriginCountrySelectList = new SelectList(await _originCountryService.GetAllAsync(), "Id", "Name");
                ViewBag.ColorSelectList = new SelectList(await _colorService.GetAllAsync(), "Id", "Name");
                return View("ProductDetailCreate", model);
            }

            // ensure FK - use Id properties from model
            var detail = ProductDetail.Create(productId,
                model.LengthId,
                model.SurfaceFinishId,
                model.HardnessId,
                model.ElasticityId,
                model.OriginCountryId,
                model.ColorId,
                model.Price,
                model.StockQuantity);

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

            var items = (await _productService.SuggestAsync(query, 12)).ToList();

            var list = new List<object>(items.Count);
            foreach (var p in items)
            {
                decimal? min = null, max = null;
                try
                {
                    var details = await _productDetailService.GetByProductIdAsync(p.Id);
                    foreach (var d in details)
                    {
                        if (min == null || d.Price < min) min = d.Price;
                        if (max == null || d.Price > max) max = d.Price;
                    }
                }
                catch { }

                list.Add(new { p.Id, p.Name, p.ProductCode, p.Thumbnail, priceMin = min, priceMax = max });
            }

            return Json(list);
        }
    }
}
