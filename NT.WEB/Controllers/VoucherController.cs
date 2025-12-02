using Microsoft.AspNetCore.Mvc;

namespace NT.WEB.Controllers
{
    public class VoucherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
