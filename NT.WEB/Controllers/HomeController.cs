using Microsoft.AspNetCore.Mvc;
using NT.WEB.Models;
using System.Diagnostics;

namespace NT.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly NT.WEB.Services.ProductWebService _productService;
        private readonly NT.WEB.Services.ProductDetailWebService _productDetailService;
        private readonly NT.WEB.Services.BrandWebService _brandService;

        public HomeController(ILogger<HomeController> logger,
                              NT.WEB.Services.ProductWebService productService,
                              NT.WEB.Services.ProductDetailWebService productDetailService,
                              NT.WEB.Services.BrandWebService brandService)
        {
            _logger = logger;
            _productService = productService;
            _productDetailService = productDetailService;
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var clientLayoutItems = new List<dynamic>();
            var allProducts = await _productService.GetAllAsync();
            foreach (var p in allProducts)
            {
                var brand = await _brandService.GetByIdAsync(p.BrandId);
                var details = await _productDetailService.GetWithLookupsByProductIdAsync(p.Id);
                var firstDetail = details?.FirstOrDefault();
                var firstImg = firstDetail?.Images?.FirstOrDefault();
                clientLayoutItems.Add(new
                {
                    p.Id,
                    p.Name,
                    BrandName = brand?.Name,
                    Thumbnail = !string.IsNullOrWhiteSpace(p.Thumbnail) ? p.Thumbnail : (firstImg?.ImageUrl ?? p.Thumbnail),
                    Price = firstDetail?.Price
                });
                if (clientLayoutItems.Count >= 12) break;
            }
            ViewBag.ClientLayoutProducts = clientLayoutItems;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult IndexAdmin()
        {
            return View();
        }

        /// <summary>
        /// Trang thông báo không có quyền truy cập
        /// </summary>
        public IActionResult AccessDenied(string? resource = null, string? action = null)
        {
            ViewBag.Resource = resource;
            ViewBag.Action = action;
            return View();
        }

        // Returns all products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string? q = null, Guid? brandId = null, Guid? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var products = await _productService.GetAllAsync();

            if (brandId.HasValue && brandId.Value != Guid.Empty)
                products = products.Where(p => p.BrandId == brandId.Value).ToList();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim().ToLowerInvariant();
                products = products.Where(p =>
                    (!string.IsNullOrWhiteSpace(p.Name) && p.Name.ToLowerInvariant().Contains(term)) ||
                    (!string.IsNullOrWhiteSpace(p.ProductCode) && p.ProductCode.ToLowerInvariant().Contains(term))
                ).ToList();
            }

            var result = new List<object>();
            foreach (var p in products)
            {
                var brand = await _brandService.GetByIdAsync(p.BrandId);
                var details = await _productDetailService.GetWithLookupsByProductIdAsync(p.Id) ?? Enumerable.Empty<NT.SHARED.Models.ProductDetail>();

                // Filter by category if provided (assuming detail has Categories or CategoryId)
                if (categoryId.HasValue && categoryId.Value != Guid.Empty)
                {
                    details = details
                        .Where(d =>
                        {
                            try
                            {
                                // Try common shapes: d.CategoryId or d.Categories (collection of ids)
                                Guid catId = (Guid)(d.GetType().GetProperty("CategoryId")?.GetValue(d) ?? Guid.Empty);
                                if (catId != Guid.Empty) return catId == categoryId.Value;
                                var cats = d.GetType().GetProperty("Categories")?.GetValue(d) as IEnumerable<Guid>;
                                return cats != null && cats.Contains(categoryId.Value);
                            }
                            catch { return false; }
                        })
                        .ToList();
                }

                // Filter by price range if provided
                if (minPrice.HasValue || maxPrice.HasValue)
                {
                    details = details
                        .Where(d =>
                        {
                            try
                            {
                                decimal? price = (decimal?)d.GetType().GetProperty("Price")?.GetValue(d);
                                if (!price.HasValue) return false;
                                if (minPrice.HasValue && price.Value < minPrice.Value) return false;
                                if (maxPrice.HasValue && price.Value > maxPrice.Value) return false;
                                return true;
                            }
                            catch { return false; }
                        })
                        .ToList();
                }

                var firstDetail = details.FirstOrDefault();
                var images = firstDetail?.Images ?? new List<NT.SHARED.Models.ProductImage>();
                var firstImg = images.FirstOrDefault();

                decimal? priceMin = null, priceMax = null;
                try
                {
                    var prices = details
                        .Select(d => (decimal?)d.Price)
                        .Where(v => v.HasValue)
                        .Select(v => v!.Value)
                        .ToList();
                    if (prices.Count > 0)
                    {
                        priceMin = prices.Min();
                        priceMax = prices.Max();
                    }
                }
                catch { }

                result.Add(new
                {
                    id = p.Id,
                    name = p.Name,
                    productCode = p.ProductCode,
                    brandName = brand?.Name,
                    thumbnail = !string.IsNullOrWhiteSpace(p.Thumbnail) ? p.Thumbnail : (firstImg?.GetType().GetProperty("ImageUrl")?.GetValue(firstImg)?.ToString() ?? p.Thumbnail),
                    priceMin,
                    priceMax
                });
            }

            // Render a view instead of returning raw JSON
            return View(result);
        }
    }
}
