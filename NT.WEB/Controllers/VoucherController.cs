using Microsoft.AspNetCore.Mvc;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using System;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class VoucherController : Controller
    {
        private readonly IVocherService _service;

        public VoucherController(IVocherService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: Voucher
        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        // GET: Voucher/Details/{id}
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null || id == Guid.Empty) return BadRequest();

            var voucher = await _service.GetByIdAsync(id.Value);
            if (voucher is null) return NotFound();

            return View(voucher);
        }

        // GET: Voucher/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voucher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Voucher voucher)
        {
            // Server-side validation: StartDate < EndDate and both in the future
            if (voucher.StartDate.HasValue)
            {
                voucher.StartDate = DateTime.SpecifyKind(voucher.StartDate.Value, DateTimeKind.Utc);
                if (voucher.StartDate.Value <= DateTime.UtcNow)
                    ModelState.AddModelError(nameof(voucher.StartDate), "Ngày bắt đầu phải trong tương lai");
            }
            if (voucher.EndDate.HasValue)
            {
                voucher.EndDate = DateTime.SpecifyKind(voucher.EndDate.Value, DateTimeKind.Utc);
                if (voucher.EndDate.Value <= DateTime.UtcNow)
                    ModelState.AddModelError(nameof(voucher.EndDate), "Ngày kết thúc phải trong tương lai");
            }
            if (voucher.StartDate.HasValue && voucher.EndDate.HasValue && voucher.StartDate.Value >= voucher.EndDate.Value)
            {
                ModelState.AddModelError(nameof(voucher.EndDate), "Ngày bắt đầu phải trước ngày kết thúc");
            }

            if (!ModelState.IsValid) return View(voucher);

            await _service.AddAsync(voucher);
            return RedirectToAction(nameof(Index));
        }

        // GET: Voucher/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null || id == Guid.Empty) return BadRequest();

            var voucher = await _service.GetByIdAsync(id.Value);
            if (voucher is null) return NotFound();

            return View(voucher);
        }

        // POST: Voucher/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Voucher voucher)
        {
            if (id == Guid.Empty || voucher is null || id != voucher.Id) return BadRequest();

            // Server-side validation: StartDate < EndDate and both in the future
            if (voucher.StartDate.HasValue)
            {
                voucher.StartDate = DateTime.SpecifyKind(voucher.StartDate.Value, DateTimeKind.Utc);
                if (voucher.StartDate.Value <= DateTime.UtcNow)
                    ModelState.AddModelError(nameof(voucher.StartDate), "Ngày bắt đầu phải trong tương lai");
            }
            if (voucher.EndDate.HasValue)
            {
                voucher.EndDate = DateTime.SpecifyKind(voucher.EndDate.Value, DateTimeKind.Utc);
                if (voucher.EndDate.Value <= DateTime.UtcNow)
                    ModelState.AddModelError(nameof(voucher.EndDate), "Ngày kết thúc phải trong tương lai");
            }
            if (voucher.StartDate.HasValue && voucher.EndDate.HasValue && voucher.StartDate.Value >= voucher.EndDate.Value)
            {
                ModelState.AddModelError(nameof(voucher.EndDate), "Ngày bắt đầu phải trước ngày kết thúc");
            }

            if (!ModelState.IsValid) return View(voucher);

            try
            {
                await _service.UpdateAsync(voucher);
            }
            catch (Exception)
            {
                // Optionally log exception
                ModelState.AddModelError(string.Empty, "không thể lưu, hãy thử lại");
                return View(voucher);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Voucher/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null || id == Guid.Empty) return BadRequest();

            var voucher = await _service.GetByIdAsync(id.Value);
            if (voucher is null) return NotFound();

            return View(voucher);
        }

        // POST: Voucher/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return BadRequest();

            return RedirectToAction(nameof(Index));
        }
    }
}
