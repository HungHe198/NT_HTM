using Microsoft.AspNetCore.Mvc;

namespace NT.WEB.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
