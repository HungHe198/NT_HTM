using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.WEB.Services;

namespace NT.WEB.ViewComponents
{
    public class FeaturedProductsViewComponent : ViewComponent
    {
        private readonly ProductWebService _productService;
        private readonly BrandWebService _brandService;

        public FeaturedProductsViewComponent(ProductWebService productService, BrandWebService brandService)
        {
            _productService = productService;
            _brandService = brandService;
        }

        // Return all products (or empty list)
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _productService.GetAllAsync();
            var list = new List<FeaturedProductItem>();
            foreach (var p in items)
            {
                var brand = await _brandService.GetByIdAsync(p.BrandId);
                list.Add(new FeaturedProductItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    BrandName = brand?.Name,
                    Thumbnail = p.Thumbnail
                });
            }
            return View(list);
        }

        public class FeaturedProductItem
        {
            public System.Guid Id { get; set; }
            public string Name { get; set; } = null!;
            public string? BrandName { get; set; }
            public string? Thumbnail { get; set; }
        }
    }
}
