using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Authorization;
using NT.WEB.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    /// <summary>
    /// Controller quản lý khách hàng - dành cho Admin/Employee
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class CustomerController : Controller
    {
        private readonly CustomerWebService _service;

        public CustomerController(CustomerWebService service)
        {
            _service = service;
        }

        [RequirePermission("Customer", "Index")]
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

        [RequirePermission("Customer", "Details")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [RequirePermission("Customer", "Create")]
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
        [RequirePermission("Customer", "Create")]
        public async Task<IActionResult> Create(Customer model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.AddAsync(model);
            await _service.SaveChangesAsync();
            TempData["Success"] = "Đã tạo khách hàng thành công";
            return RedirectToAction(nameof(Index));
        }

        [RequirePermission("Customer", "Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("Customer", "Edit")]
        public async Task<IActionResult> Edit(Guid id, Customer model)
        {
            if (id == Guid.Empty || model == null || id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _service.UpdateAsync(model);
            await _service.SaveChangesAsync();
            TempData["Success"] = "Đã cập nhật thông tin khách hàng";
            return RedirectToAction(nameof(Index));
        }

        [RequirePermission("Customer", "Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RequirePermission("Customer", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            await _service.DeleteAsync(id);
            await _service.SaveChangesAsync();
            TempData["Success"] = "Đã xóa khách hàng";
            return RedirectToAction(nameof(Index));
        }
    }
}
