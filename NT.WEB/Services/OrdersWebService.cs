using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    /// <summary>
    /// Facade web service that exposes CRUD helpers for order-related entities
    /// using the existing GenericService implementations.
    /// </summary>
    public class OrdersWebService
    {
        private readonly GenericService<Order> _orderService;
        private readonly GenericService<OrderDetail> _orderDetailService;
        private readonly GenericService<Customer> _customerService;
        private readonly GenericService<Coupon> _couponService; // voucher
        private readonly GenericService<ProductDetail> _productDetailService;

        public OrdersWebService(
            IGenericRepository<Order> orderRepository,
            IGenericRepository<OrderDetail> orderDetailRepository,
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<Coupon> couponRepository,
            IGenericRepository<ProductDetail> productDetailRepository)
        {
            _orderService = new GenericService<Order>(orderRepository ?? throw new ArgumentNullException(nameof(orderRepository)));
            _orderDetailService = new GenericService<OrderDetail>(orderDetailRepository ?? throw new ArgumentNullException(nameof(orderDetailRepository)));
            _customerService = new GenericService<Customer>(customerRepository ?? throw new ArgumentNullException(nameof(customerRepository)));
            _couponService = new GenericService<Coupon>(couponRepository ?? throw new ArgumentNullException(nameof(couponRepository)));
            _productDetailService = new GenericService<ProductDetail>(productDetailRepository ?? throw new ArgumentNullException(nameof(productDetailRepository)));
        }

        // ---------------- Orders ----------------
        public Task<IEnumerable<Order>> GetAllOrdersAsync() => _orderService.GetAllAsync();
        public Task<Order?> GetOrderByIdAsync(Guid id) => _orderService.GetByIdAsync(id);
        public Task<Order> AddOrderAsync(Order order) => _orderService.AddAsync(order);
        public Task<Order> UpdateOrderAsync(Order order) => _orderService.UpdateAsync(order);
        public Task<bool> DeleteOrderAsync(Guid id) => _orderService.DeleteAsync(id);
        public Task SaveOrderChangesAsync() => _orderService.SaveChangesAsync();

        // ---------------- OrderDetails ----------------
        public Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync() => _orderDetailService.GetAllAsync();
        public Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id) => _orderDetailService.GetByIdAsync(id);
        public Task<OrderDetail> AddOrderDetailAsync(OrderDetail detail) => _orderDetailService.AddAsync(detail);
        public Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail detail) => _orderDetailService.UpdateAsync(detail);
        public Task<bool> DeleteOrderDetailAsync(Guid id) => _orderDetailService.DeleteAsync(id);
        public Task SaveOrderDetailChangesAsync() => _orderDetailService.SaveChangesAsync();

        // ---------------- Customers ----------------
        public Task<IEnumerable<Customer>> GetAllCustomersAsync() => _customerService.GetAllAsync();
        public Task<Customer?> GetCustomerByIdAsync(Guid id) => _customerService.GetByIdAsync(id);
        public Task<Customer> AddCustomerAsync(Customer customer) => _customerService.AddAsync(customer);
        public Task<Customer> UpdateCustomerAsync(Customer customer) => _customerService.UpdateAsync(customer);
        public Task<bool> DeleteCustomerAsync(Guid id) => _customerService.DeleteAsync(id);
        public Task SaveCustomerChangesAsync() => _customerService.SaveChangesAsync();

        // ---------------- Coupons (vouchers) ----------------
        public Task<IEnumerable<Coupon>> GetAllCouponsAsync() => _couponService.GetAllAsync();
        public Task<Coupon?> GetCouponByIdAsync(Guid id) => _couponService.GetByIdAsync(id);
        public Task<Coupon> AddCouponAsync(Coupon coupon) => _couponService.AddAsync(coupon);
        public Task<Coupon> UpdateCouponAsync(Coupon coupon) => _couponService.UpdateAsync(coupon);
        public Task<bool> DeleteCouponAsync(Guid id) => _couponService.DeleteAsync(id);
        public Task SaveCouponChangesAsync() => _couponService.SaveChangesAsync();

        // ---------------- ProductDetails ----------------
        public Task<IEnumerable<ProductDetail>> GetAllProductDetailsAsync() => _productDetailService.GetAllAsync();
        public Task<ProductDetail?> GetProductDetailByIdAsync(Guid id) => _productDetailService.GetByIdAsync(id);
        public Task<ProductDetail> AddProductDetailAsync(ProductDetail detail) => _productDetailService.AddAsync(detail);
        public Task<ProductDetail> UpdateProductDetailAsync(ProductDetail detail) => _productDetailService.UpdateAsync(detail);
        public Task<bool> DeleteProductDetailAsync(Guid id) => _productDetailService.DeleteAsync(id);
        public Task SaveProductDetailChangesAsync() => _productDetailService.SaveChangesAsync();
    }
}
