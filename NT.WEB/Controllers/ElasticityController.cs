using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;

namespace NT.WEB.Controllers
{
    public class ElasticityController : Controller
    {
        private readonly ElasticityWebService _service;

        public ElasticityController(ElasticityWebService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<IActionResult> Index(string? q)
        {
            var model = string.IsNullOrWhiteSpace(q)
                ? await _service.GetAllAsync()
                : await _service.SearchByNameAsync(q);
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Elasticity model)
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
            if (item is null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Elasticity model)
        {
            if (id == Guid.Empty || model is null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _service.UpdateAsync(model);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();
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

        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            var result = await _service.SearchByNameAsync(q);
            return Json(result);
        }
    }
}