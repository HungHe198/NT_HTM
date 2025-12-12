using Microsoft.AspNetCore.Mvc;
using NT.SHARED.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.ViewComponents
{
    public class OrdersPendingCountViewComponent : ViewComponent
    {
        private readonly NT.BLL.Interfaces.IGenericRepository<Order> _orderRepo;
        public OrdersPendingCountViewComponent(NT.BLL.Interfaces.IGenericRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            var count = (orders ?? Array.Empty<Order>())
                .Count(o => string.Equals(o.Status ?? "0", "0", StringComparison.Ordinal));
            return View("Default", count);
        }
    }
}
