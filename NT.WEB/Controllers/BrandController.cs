using Microsoft.AspNetCore.Mvc;

namespace NT.WEB.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
