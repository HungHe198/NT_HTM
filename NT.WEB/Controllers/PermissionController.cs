using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;
using System;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class PermissionController : Controller
    {
        private readonly PermissionWebService _service;

        public PermissionController(PermissionWebService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Permission model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.AddAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(Guid id, Permission model)
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
