using Microsoft.AspNetCore.Mvc;
using NT.WEB.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NT.SHARED.Models;

namespace NT.WEB.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly CartWebService _cartService;
        private readonly CartDetailWebService _cartDetailService;
        private readonly CustomerWebService _customerService;

        public CartSummaryViewComponent(CartWebService cartService, CartDetailWebService cartDetailService, CustomerWebService customerService)
        {
            _cartService = cartService;
            _cartDetailService = cartDetailService;
            _customerService = customerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdClaim = HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return View(new List<CartDetail>());
            }

            var customers = await _customerService.FindAsync(c => c.UserId == userId);
            var customer = customers?.FirstOrDefault();
            if (customer == null)
            {
                return View(new List<CartDetail>());
            }

            var carts = await _cartService.FindAsync(ct => ct.CustomerId == customer.Id);
            var cart = carts?.FirstOrDefault();
            if (cart == null)
            {
                return View(new List<CartDetail>());
            }

            var cartItems = await _cartDetailService.FindAsync(cd => cd.CartId == cart.Id);
            var items = (cartItems ?? new List<CartDetail>()).Where(ci => ci.Quantity > 0).ToList();
            ViewData["CartId"] = cart.Id;
            return View(items);
        }
    }
}
