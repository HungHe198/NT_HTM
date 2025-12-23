using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Constants;
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
            // Chỉ lấy sản phẩm có trạng thái hoạt động
            var activeProducts = allProducts.Where(p => p.Status == ProductStatus.Active).ToList();
            foreach (var p in activeProducts)
            {
                var brand = await _brandService.GetByIdAsync(p.BrandId);
                var allDetails = await _productDetailService.GetWithLookupsByProductIdAsync(p.Id);
                // Chỉ lấy các biến thể có IsActive = true và StockQuantity > 0
                var details = allDetails?.Where(d => d.IsActive && d.StockQuantity > 0).ToList();
                
                // Bỏ qua sản phẩm không có biến thể hoạt động
                if (details == null || !details.Any()) continue;
                
                var firstDetail = details.FirstOrDefault();
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
            // === PRICE VALIDATION ===
            // Validate minPrice: không được âm
            if (minPrice.HasValue && minPrice.Value < 0)
            {
                minPrice = 0;
                TempData["Warning"] = "Giá tối thiểu không được âm, đã tự động điều chỉnh về 0.";
            }

            // Validate maxPrice: không được âm
            if (maxPrice.HasValue && maxPrice.Value < 0)
            {
                maxPrice = null; // Bỏ qua giá trị âm
                TempData["Warning"] = "Giá tối đa không được âm, đã bỏ qua bộ lọc này.";
            }

            // Validate: minPrice không được lớn hơn maxPrice
            if (minPrice.HasValue && maxPrice.HasValue && minPrice.Value > maxPrice.Value)
            {
                // Hoán đổi giá trị
                (minPrice, maxPrice) = (maxPrice, minPrice);
                TempData["Warning"] = "Giá tối thiểu lớn hơn giá tối đa, đã tự động hoán đổi.";
            }

            // Lưu giá trị đã validate vào ViewBag để hiển thị lại trên form
            ViewBag.ValidatedMinPrice = minPrice;
            ViewBag.ValidatedMaxPrice = maxPrice;
            
            // Flag để biết có đang lọc giá hay không
            bool isPriceFiltering = minPrice.HasValue || maxPrice.HasValue;
            ViewBag.IsPriceFiltering = isPriceFiltering;

            var products = await _productService.GetAllAsync();
            
            // Chỉ lấy sản phẩm có trạng thái hoạt động
            products = products.Where(p => p.Status == ProductStatus.Active).ToList();

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

            var matchedProducts = new List<object>();    // Sản phẩm đạt yêu cầu lọc giá
            var otherProducts = new List<object>();      // Sản phẩm còn lại (Liên hệ hoặc ngoài khoảng giá)

            foreach (var p in products)
            {
                var brand = await _brandService.GetByIdAsync(p.BrandId);
                var allDetailsRaw = await _productDetailService.GetWithLookupsByProductIdAsync(p.Id) ?? Enumerable.Empty<NT.SHARED.Models.ProductDetail>();
                // Chỉ lấy các biến thể có IsActive = true và StockQuantity > 0
                var details = allDetailsRaw.Where(d => d.IsActive && d.StockQuantity > 0).ToList();
                
                // Bỏ qua sản phẩm không có biến thể hoạt động
                if (!details.Any()) continue;

                // Filter by category if provided
                if (categoryId.HasValue && categoryId.Value != Guid.Empty)
                {
                    details = details
                        .Where(d =>
                        {
                            try
                            {
                                Guid catId = (Guid)(d.GetType().GetProperty("CategoryId")?.GetValue(d) ?? Guid.Empty);
                                if (catId != Guid.Empty) return catId == categoryId.Value;
                                var cats = d.GetType().GetProperty("Categories")?.GetValue(d) as IEnumerable<Guid>;
                                return cats != null && cats.Contains(categoryId.Value);
                            }
                            catch { return false; }
                        })
                        .ToList();
                }

                // Lấy tất cả giá của sản phẩm (trước khi lọc)
                var allPrices = details
                    .Select(d => (decimal?)d.Price)
                    .Where(v => v.HasValue)
                    .Select(v => v!.Value)
                    .ToList();

                decimal? priceMin = allPrices.Count > 0 ? allPrices.Min() : null;
                decimal? priceMax = allPrices.Count > 0 ? allPrices.Max() : null;

                // Lấy thumbnail
                var firstDetail = details.FirstOrDefault();
                var images = firstDetail?.Images ?? new List<NT.SHARED.Models.ProductImage>();
                var firstImg = images.FirstOrDefault();
                var thumbnail = !string.IsNullOrWhiteSpace(p.Thumbnail) 
                    ? p.Thumbnail 
                    : (firstImg?.GetType().GetProperty("ImageUrl")?.GetValue(firstImg)?.ToString() ?? p.Thumbnail);

                var productData = new
                {
                    id = p.Id,
                    name = p.Name,
                    productCode = p.ProductCode,
                    brandName = brand?.Name,
                    thumbnail,
                    priceMin,
                    priceMax
                };

                // Phân loại sản phẩm khi có lọc giá
                if (isPriceFiltering)
                {
                    // Sản phẩm không có giá (Liên hệ) → vào danh sách "khác"
                    if (!priceMin.HasValue && !priceMax.HasValue)
                    {
                        otherProducts.Add(productData);
                    }
                    else
                    {
                        // Kiểm tra có detail nào thỏa mãn điều kiện giá không
                        bool hasMatchingPrice = details.Any(d =>
                        {
                            try
                            {
                                decimal? price = (decimal?)d.Price;
                                if (!price.HasValue) return false;
                                if (minPrice.HasValue && price.Value < minPrice.Value) return false;
                                if (maxPrice.HasValue && price.Value > maxPrice.Value) return false;
                                return true;
                            }
                            catch { return false; }
                        });

                        if (hasMatchingPrice)
                        {
                            matchedProducts.Add(productData);
                        }
                        else
                        {
                            otherProducts.Add(productData);
                        }
                    }
                }
                else
                {
                    // Không lọc giá → tất cả vào danh sách chính
                    matchedProducts.Add(productData);
                }
            }

            ViewBag.MatchedProducts = matchedProducts;
            ViewBag.OtherProducts = otherProducts;

            // Render a view instead of returning raw JSON
            return View(matchedProducts);
        }
    }
}
