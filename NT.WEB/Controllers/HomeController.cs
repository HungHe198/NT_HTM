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
    }
}
