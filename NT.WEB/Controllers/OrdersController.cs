using Microsoft.AspNetCore.Mvc;

namespace NT.WEB.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
