using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerWebService _service;

        public CustomerController(CustomerWebService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsyncWithUser();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Suggest(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return Json(new { });
            q = q.Trim().ToLower();
            
            var customers = await _service.GetAllAsyncWithUser();
            var results = customers?
                .Where(c => (c.User?.Fullname ?? "").ToLower().Contains(q) ||
                            (c.User?.Email ?? "").ToLower().Contains(q) ||
                            (c.User?.PhoneNumber ?? "").ToLower().Contains(q))
                .Take(10)
                .Select(c => new
                {
                    id = c.Id,
                    fullname = c.User?.Fullname ?? "N/A",
                    email = c.User?.Email ?? "N/A",
                    phoneNumber = c.User?.PhoneNumber ?? "N/A"
                })
                .ToList() ?? new();
   
            return Json(results);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create(Guid? userId = null)
        {
            var model = new Customer();
            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                model.UserId = userId.Value;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.AddAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Customer model)
        {
            if (id == Guid.Empty || model == null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _service.UpdateAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            await _service.DeleteAsync(id);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
