using Microsoft.AspNetCore.Mvc;

namespace NT.WEB.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
