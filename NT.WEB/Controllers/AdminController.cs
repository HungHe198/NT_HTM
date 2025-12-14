using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using NT.WEB.Services;
using System;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminWebService _service;
        private readonly NT.BLL.Interfaces.IGenericRepository<Order> _orderRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<OrderDetail> _orderDetailRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<ProductDetail> _productDetailRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<User> _userRepo;

        public AdminController(
            AdminWebService service,
            NT.BLL.Interfaces.IGenericRepository<Order> orderRepo,
            NT.BLL.Interfaces.IGenericRepository<OrderDetail> orderDetailRepo,
            NT.BLL.Interfaces.IGenericRepository<ProductDetail> productDetailRepo,
            NT.BLL.Interfaces.IGenericRepository<User> userRepo)
        {
            _service = service;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _productDetailRepo = productDetailRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        // Chart API: Revenue per day (UTC date group)
        [HttpGet]
        public async Task<IActionResult> RevenueDaily(int days = 14)
        {
            var orders = await _orderRepo.GetAllAsync() ?? Array.Empty<Order>();
            var from = DateTime.UtcNow.Date.AddDays(-Math.Max(1, days) + 1);
            var data = orders
                .Where(o => o.CreatedTime.Date >= from)
                .GroupBy(o => o.CreatedTime.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { date = g.Key.ToString("yyyy-MM-dd"), revenue = g.Sum(x => x.FinalAmount) })
                .ToList();
            return Json(data);
        }

        // Chart API: Top selling product details by quantity
        [HttpGet]
        public async Task<IActionResult> TopSellingProductDetails(int top = 10)
        {
            var details = await _orderDetailRepo.GetAllAsync() ?? Array.Empty<OrderDetail>();
            var productDetails = await _productDetailRepo.GetAllAsync() ?? Array.Empty<ProductDetail>();
            var joined = details
                .GroupBy(d => d.ProductDetailId)
                .Select(g => new { ProductDetailId = g.Key, quantity = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.quantity)
                .Take(Math.Max(1, top))
                .ToList();

            var nameMap = productDetails.ToDictionary(p => p.Id, p => p.ProductId.ToString());
            var result = joined.Select(j => new
            {
                id = j.ProductDetailId,
                name = nameMap.TryGetValue(j.ProductDetailId, out var n) ? n : j.ProductDetailId.ToString(),
                quantity = j.quantity
            });

            return Json(result);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userRepo.GetAllAsync();
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Admin model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _userRepo.GetAllAsync();
                ViewBag.Users = users;
                return View(model);
            }
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
        public async Task<IActionResult> Edit(Guid id, Admin model)
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
