using Microsoft.AspNetCore.Mvc;

namespace NT.WEB.Controllers
{
    public class PaymentMethodController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
