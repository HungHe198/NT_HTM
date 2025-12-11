using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    
    public class PaymentMethodsController : Controller
    {
        private readonly NT.BLL.Interfaces.IGenericRepository<PaymentMethod> _repo;

        public PaymentMethodsController(NT.BLL.Interfaces.IGenericRepository<PaymentMethod> repo)
        {
            _repo = repo;
        }

        // GET: /PaymentMethods
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _repo.GetAllAsync();
            return View(items);
        }

        // GET: /PaymentMethods/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var items = await _repo.FindAsync(pm => pm.Id == id);
            var item = items?.FirstOrDefault();
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /PaymentMethods/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /PaymentMethods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string? description)
        {
            try
            {
                var pm = PaymentMethod.Create(name, description);
                await _repo.AddAsync(pm);
                await _repo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        // GET: /PaymentMethods/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var items = await _repo.FindAsync(pm => pm.Id == id);
            var pm = items?.FirstOrDefault();
            if (pm == null) return NotFound();
            return View(pm);
        }

        // POST: /PaymentMethods/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PaymentMethod model)
        {
            if (id == Guid.Empty || model == null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Normalize fields
            model.Name = model.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "Vui lòng nhập tên phương thức thanh toán");
                return View(model);
            }
            model.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description!.Trim();
            await _repo.UpdateAsync(model);
            await _repo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /PaymentMethods/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var items = await _repo.FindAsync(pm => pm.Id == id);
            var pm = items?.FirstOrDefault();
            if (pm == null) return NotFound();
            return View(pm);
        }

        // POST: /PaymentMethods/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
