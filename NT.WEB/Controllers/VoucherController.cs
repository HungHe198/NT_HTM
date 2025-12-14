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
            // Validate EndDate phải là tương lai (sử dụng local time)
            if (voucher.EndDate.HasValue && voucher.EndDate.Value <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(voucher.EndDate), "Ngày kết thúc phải là thời gian trong tương lai");
            }

            if (voucher.StartDate.HasValue && voucher.EndDate.HasValue && voucher.StartDate.Value >= voucher.EndDate.Value)
            {
                ModelState.AddModelError(nameof(voucher.StartDate), "Ngày bắt đầu phải trước ngày kết thúc");
            }

            // Validate DiscountPercentage
            if (voucher.DiscountPercentage.HasValue && (voucher.DiscountPercentage.Value < 0 || voucher.DiscountPercentage.Value > 100))
            {
                ModelState.AddModelError(nameof(voucher.DiscountPercentage), "Phần trăm giảm giá phải từ 0 đến 100");
            }

            // Validate MaxDiscountAmount phải nhỏ hơn MinOrderAmount
            if (voucher.MaxDiscountAmount.HasValue && voucher.MinOrderAmount.HasValue 
                && voucher.MaxDiscountAmount.Value >= voucher.MinOrderAmount.Value)
            {
                ModelState.AddModelError(nameof(voucher.MaxDiscountAmount), "Tiền giảm tối đa phải nhỏ hơn đơn hàng tối thiểu");
            }

            if (!ModelState.IsValid) return View(voucher);

            voucher.UsageCount = 0;
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

            // Validate StartDate và EndDate (giữ nguyên local time)
            if (voucher.StartDate.HasValue && voucher.EndDate.HasValue && voucher.StartDate.Value >= voucher.EndDate.Value)
            {
                ModelState.AddModelError(nameof(voucher.StartDate), "Ngày bắt đầu phải trước ngày kết thúc");
            }

            // Validate DiscountPercentage
            if (voucher.DiscountPercentage.HasValue && (voucher.DiscountPercentage.Value < 0 || voucher.DiscountPercentage.Value > 100))
            {
                ModelState.AddModelError(nameof(voucher.DiscountPercentage), "Phần trăm giảm giá phải từ 0 đến 100");
            }

            // Validate MaxDiscountAmount phải nhỏ hơn MinOrderAmount
            if (voucher.MaxDiscountAmount.HasValue && voucher.MinOrderAmount.HasValue 
                && voucher.MaxDiscountAmount.Value >= voucher.MinOrderAmount.Value)
            {
                ModelState.AddModelError(nameof(voucher.MaxDiscountAmount), "Tiền giảm tối đa phải nhỏ hơn đơn hàng tối thiểu");
            }

            if (!ModelState.IsValid) return View(voucher);

            try
            {
                await _service.UpdateAsync(voucher);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Không thể lưu, hãy thử lại");
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
