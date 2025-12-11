using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    public class OrdersController : Controller
    {
        private readonly NT.BLL.Interfaces.IGenericRepository<Order> _orderRepo;
        private readonly NT.BLL.Interfaces.IGenericRepository<OrderDetail> _orderDetailRepo;

        public OrdersController(NT.BLL.Interfaces.IGenericRepository<Order> orderRepo,
                                 NT.BLL.Interfaces.IGenericRepository<OrderDetail> orderDetailRepo)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
        }

        
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepo.GetAllAsync();
            return View(orders ?? Array.Empty<Order>());
        }

        // Customer/admin can review an order details
        [HttpGet]
        public async Task<IActionResult> Review(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var orders = await _orderRepo.FindAsync(o => o.Id == id);
            var order = orders?.FirstOrDefault();
            if (order == null) return NotFound();
            var details = await _orderDetailRepo.FindAsync(d => d.OrderId == id);
            ViewBag.Details = details ?? Array.Empty<OrderDetail>();
            return View(order);
        }

        // Admin: update status (0=pending,1=confirmed,2=shipping,3=delivered => then set -1 closed)
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid id, string status)
        {
            if (id == Guid.Empty) return BadRequest();
            var orders = await _orderRepo.FindAsync(o => o.Id == id);
            var order = orders?.FirstOrDefault();
            if (order == null) return NotFound();

            order.Status = status;
            // If delivered (3), set to -1 as closed per requirement
            if (status == "3")
            {
                order.Status = "-1";
            }
            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Review), new { id });
        }

        // Cancel order with note (admin/employee/customer). Note is required on cancel.
        [Authorize(Roles = "Admin,Employee,Customer")]
        [HttpPost]
        public async Task<IActionResult> Cancel(Guid id, string note)
        {
            if (id == Guid.Empty) return BadRequest();
            note = note?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(note))
            {
                TempData["Error"] = "Vui lòng ghi lý do hủy đơn";
                return RedirectToAction(nameof(Review), new { id });
            }
            var orders = await _orderRepo.FindAsync(o => o.Id == id);
            var order = orders?.FirstOrDefault();
            if (order == null) return NotFound();

            order.Note = note; // hidden from customer during placement, only used for cancellation reason
            order.Status = "4"; // canceled
            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();
            TempData["Success"] = "Đã hủy đơn hàng";
            return RedirectToAction(nameof(Review), new { id });
        }
    }
}
