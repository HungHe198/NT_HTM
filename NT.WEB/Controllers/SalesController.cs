using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NT.BLL.Interfaces;
using NT.SHARED.Models;
using NT.WEB.Authorization;
using NT.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NT.WEB.Controllers
{
    /// <summary>
    /// Bán hàng tại quầy (POS - Point of Sale) cho nhân viên bán hàng
    /// Hỗ trợ: Khách vãng lai, tạo tài khoản nhanh, giỏ hàng chờ (pending orders)
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class SalesController : Controller
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductDetail> _productDetailRepo;
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepo;
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IGenericRepository<PaymentMethod> _paymentMethodRepo;
        private readonly IGenericRepository<Voucher> _voucherRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly CustomerWebService _customerService;

        // Status constants cho Order
        private const string ORDER_STATUS_POS_PENDING = "-2"; // Đơn chờ POS (đã trừ kho)
        private const string ORDER_STATUS_PENDING = "0";      // Đơn online chờ xác nhận (chưa trừ kho)
        private const string ORDER_STATUS_CONFIRMED = "1";    // Đã xác nhận
        private const string ORDER_STATUS_SHIPPING = "2";     // Đang giao
        private const string ORDER_STATUS_COMPLETED = "3";    // Hoàn thành

        public SalesController(
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductDetail> productDetailRepo,
            IGenericRepository<Order> orderRepo,
            IGenericRepository<OrderDetail> orderDetailRepo,
            IGenericRepository<Customer> customerRepo,
            IGenericRepository<PaymentMethod> paymentMethodRepo,
            IGenericRepository<Voucher> voucherRepo,
            IGenericRepository<User> userRepo,
            IGenericRepository<Role> roleRepo,
            IPasswordHasher<User> passwordHasher,
            CustomerWebService customerService)
        {
            _productRepo = productRepo;
            _productDetailRepo = productDetailRepo;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _customerRepo = customerRepo;
            _paymentMethodRepo = paymentMethodRepo;
            _voucherRepo = voucherRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _passwordHasher = passwordHasher;
            _customerService = customerService;
        }

        /// <summary>
        /// API: Thêm sản phẩm vào giỏ cho một Khách hàng cụ thể (POS) - tạo/append đơn chờ cho khách đó và trừ tồn kho ngay
        /// Dùng khi muốn tạo giỏ hàng/đơn chờ thuộc về một khách khác (không phải giỏ tạm client-side)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddToCartForCustomer([FromBody] AddToCartForCustomerRequest request)
        {
            if (request == null) return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            if (request.ProductDetailId == Guid.Empty)
                return Json(new { success = false, message = "Sản phẩm không hợp lệ" });

            if (request.Quantity <= 0)
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0" });

            try
            {
                var productDetail = await _productDetailRepo.GetByIdAsync(request.ProductDetailId);
                if (productDetail == null)
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });

                if (productDetail.StockQuantity < request.Quantity)
                    return Json(new { success = false, message = $"Không đủ hàng. Tồn kho còn: {productDetail.StockQuantity}" });

                // Trừ tồn kho ngay
                productDetail.StockQuantity -= request.Quantity;
                await _productDetailRepo.UpdateAsync(productDetail);

                // Tìm đơn chờ (POS) hiện có cho khách này do nhân viên hiện tại tạo (để append vào)
                var staffId = GetCurrentUserId();
                Order order = null;

                if (request.CustomerId.HasValue && request.CustomerId.Value != Guid.Empty)
                {
                    var exist = await _orderRepo.FindAsync(o => o.Status == ORDER_STATUS_POS_PENDING && o.ConfirmedByUserId == staffId && o.CustomerId == request.CustomerId.Value);
                    order = exist?.FirstOrDefault();
                }
                else
                {
                    // Try match by phone for guest-like records
                    var phone = (request.CustomerPhone ?? string.Empty).Trim();
                    var exist = await _orderRepo.FindAsync(o => o.Status == ORDER_STATUS_POS_PENDING && o.ConfirmedByUserId == staffId && o.PhoneNumber == phone);
                    order = exist?.FirstOrDefault();
                }

                // Prepare item detail
                decimal itemTotal = productDetail.Price * request.Quantity;

                if (order == null)
                {
                    // create new pending order
                    var paymentMethods = await _paymentMethodRepo.GetAllAsync();
                    var defaultPaymentMethod = paymentMethods?.FirstOrDefault();

                    order = new Order
                    {
                        CustomerId = request.CustomerId.HasValue && request.CustomerId.Value != Guid.Empty ? request.CustomerId : null,
                        PaymentMethodId = defaultPaymentMethod?.Id ?? Guid.Empty,
                        CreatedTime = DateTime.UtcNow,
                        TotalAmount = itemTotal,
                        FinalAmount = itemTotal,
                        Status = ORDER_STATUS_POS_PENDING,
                        PhoneNumber = request.CustomerPhone ?? "Khách vãng lai",
                        ShippingAddress = request.CustomerAddress ?? "Bán tại quầy",
                        Note = $"[POS Pending] {request.CustomerName ?? (request.CustomerPhone ?? "Khách vãng lai")} - Tạo lúc {DateTime.Now:HH:mm}",
                        ConfirmedByUserId = staffId
                    };

                    await _orderRepo.AddAsync(order);
                }
                else
                {
                    // append totals
                    order.TotalAmount = (order.TotalAmount) + itemTotal;
                    order.FinalAmount = order.TotalAmount;
                    await _orderRepo.UpdateAsync(order);
                }

                // create order detail
                var od = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductDetailId = productDetail.Id,
                    ProductCodeAtOrder = productDetail.Product?.ProductCode,
                    NameAtOrder = productDetail.Product?.Name ?? "Sản phẩm",
                    Quantity = request.Quantity,
                    UnitPrice = productDetail.Price,
                    LengthAtOrder = productDetail.Length?.Name,
                    ColorAtOrder = productDetail.Color?.Name,
                    HardnessAtOrder = productDetail.Hardness?.Name,
                    TotalPrice = itemTotal
                };

                await _orderDetailRepo.AddAsync(od);

                // Persist changes
                await _productDetailRepo.SaveChangesAsync();
                await _orderRepo.SaveChangesAsync();
                await _orderDetailRepo.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Đã thêm vào giỏ hàng của khách",
                    orderId = order.Id,
                    orderCode = $"POS-{order.Id.ToString().Substring(0, 8).ToUpper()}",
                    newStock = productDetail.StockQuantity
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// Trang chính POS - Bán hàng tại quầy
        /// </summary>
        [RequirePermission("Sales", "Index")]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            var paymentMethods = await _paymentMethodRepo.GetAllAsync();
            
            // Chỉ lấy sản phẩm có hàng
            var activeProducts = products?.Where(p => p.Status == "1" || string.IsNullOrEmpty(p.Status)).ToList() ?? new List<Product>();
            
            // Chỉ lấy phương thức thanh toán dành cho Offline/POS (Tiền mặt, Chuyển khoản)
            var offlinePaymentMethods = (paymentMethods ?? new List<PaymentMethod>())
                .Where(p => p.IsAvailableForOffline())
                .ToList();
            
            // Lấy các đơn hàng chờ (pending orders) của nhân viên hiện tại
            var currentUserId = GetCurrentUserId();
            var pendingOrders = await GetPendingOrdersAsync(currentUserId);
            // Available vouchers for POS (only valid ones)
            var vouchers = await _voucherRepo.GetAllAsync();
            var availableVouchers = (vouchers ?? new List<Voucher>())
                .Where(v => (v.StartDate == null || v.StartDate <= DateTime.Now) && (v.EndDate == null || v.EndDate >= DateTime.Now))
                .ToList();

            ViewBag.AvailableVouchers = availableVouchers;
            
            ViewBag.Products = activeProducts;
            ViewBag.PaymentMethods = offlinePaymentMethods;
            ViewBag.PendingOrders = pendingOrders;
            
            return View();
        }

        /// <summary>
        /// API: Tìm kiếm sản phẩm theo tên/mã (chỉ sản phẩm đang hoạt động)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SearchProducts(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return Json(new List<object>());
            }

            q = q.ToLower().Trim();
            var products = await _productRepo.GetAllAsync();
            var filtered = (products ?? Enumerable.Empty<Product>())
                // Chỉ lấy sản phẩm đang hoạt động (Status = "1" hoặc null/empty)
                .Where(p => NT.SHARED.Constants.ProductStatus.IsActive(p.Status) || string.IsNullOrEmpty(p.Status))
                .Where(p => !string.IsNullOrEmpty(p.Name) &&
                    (p.Name.ToLower().Contains(q) || 
                     (!string.IsNullOrEmpty(p.ProductCode) && p.ProductCode.ToLower().Contains(q))))
                .Select(p => new { id = p.Id, name = p.Name, productCode = p.ProductCode })
                .Take(10)
                .ToList();

            return Json(filtered);
        }

        /// <summary>
        /// API: Lấy biến thể sản phẩm (ProductDetail) theo ProductId
        /// Chỉ trả về biến thể nếu sản phẩm đang hoạt động
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProductVariants(Guid productId)
        {
            if (productId == Guid.Empty) return Json(new List<object>());

            // Kiểm tra sản phẩm có đang hoạt động không
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return Json(new List<object>());
            
            // Nếu sản phẩm ngừng bán, không cho phép mua
            if (!NT.SHARED.Constants.ProductStatus.IsActive(product.Status) && !string.IsNullOrEmpty(product.Status))
                return Json(new List<object>());

            // Load ProductDetails với các navigation properties
            var details = await _productDetailRepo.FindAsync(
                d => d.ProductId == productId && d.StockQuantity > 0,
                d => d.Length!,
                d => d.Hardness!,
                d => d.Color!
            );

            var data = details?
                .Select(d => new
                {
                    id = d.Id,
                    price = d.Price,
                    stockQuantity = d.StockQuantity,
                    productId = d.ProductId,
                    length = d.Length?.Name ?? "N/A",
                    hardness = d.Hardness?.Name ?? "N/A",
                    color = d.Color?.Name ?? "N/A"
                })
                .ToList() ?? new();

            return Json(data);
        }

        /// <summary>
        /// API: Tìm khách hàng theo SĐT
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SearchCustomer(string? q, string? phone)
        {
            var query = (q ?? phone ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(query)) return Json(null);

            // Tìm trong Customer với User - hỗ trợ tìm theo SĐT hoặc theo tên
            var customers = await _customerService.GetAllAsyncWithUser();
            var customer = (customers ?? Enumerable.Empty<NT.SHARED.Models.Customer>())
                .FirstOrDefault(c =>
                    (!string.IsNullOrWhiteSpace(c.User?.PhoneNumber) && c.User.PhoneNumber.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(c.User?.Fullname) && c.User.Fullname.Contains(query, StringComparison.OrdinalIgnoreCase))
                );

            if (customer == null) return Json(null);

            return Json(new
            {
                id = customer.Id,
                name = customer.User?.Fullname ?? "Khách hàng",
                phone = customer.User?.PhoneNumber,
                address = customer.Address ?? ""
            });
        }

        /// <summary>
        /// API: Tạo tài khoản khách hàng nhanh tại quầy
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> QuickCreateCustomer([FromBody] QuickCreateCustomerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                return Json(new { success = false, message = "Số điện thoại là bắt buộc" });

            if (string.IsNullOrWhiteSpace(request.Fullname))
                return Json(new { success = false, message = "Họ tên là bắt buộc" });

            // Kiểm tra số điện thoại đã tồn tại
            var existingUsers = await _userRepo.FindAsync(u => u.PhoneNumber == request.PhoneNumber.Trim());
            if (existingUsers.Any())
                return Json(new { success = false, message = "Số điện thoại đã được sử dụng" });

            try
            {
                // Tạo username từ số điện thoại
                var username = $"kh_{request.PhoneNumber.Trim()}";
                var existingUsername = await _userRepo.FindAsync(u => u.Username == username);
                if (existingUsername.Any())
                {
                    username = $"kh_{request.PhoneNumber.Trim()}_{DateTime.Now.Ticks % 10000}";
                }

                // Lấy Customer role
                var customerRoles = await _roleRepo.FindAsync(r => r.Name == "Customer");
                var customerRole = customerRoles.FirstOrDefault();
                if (customerRole == null)
                    return Json(new { success = false, message = "Không tìm thấy vai trò Customer" });

                // Tạo User
                var user = new User
                {
                    Username = username,
                    Fullname = request.Fullname.Trim(),
                    PhoneNumber = request.PhoneNumber.Trim(),
                    Email = request.Email?.Trim(),
                    RoleId = customerRole.Id,
                    Status = "1",
                    PasswordHash = _passwordHasher.HashPassword(null!, request.PhoneNumber.Trim()) // Mật khẩu mặc định = SĐT
                };

                await _userRepo.AddAsync(user);
                await _userRepo.SaveChangesAsync();

                // Tạo Customer
                var customer = new Customer
                {
                    UserId = user.Id,
                    Address = request.Address?.Trim()
                };

                await _customerRepo.AddAsync(customer);
                await _customerRepo.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Tạo khách hàng thành công",
                    customer = new
                    {
                        id = customer.Id,
                        name = user.Fullname,
                        phone = user.PhoneNumber,
                        address = customer.Address ?? ""
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// API: Tạo đơn hàng chờ (Pending Order) - tồn kho đã được trừ khi thêm vào giỏ
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePendingOrder([FromBody] CreatePendingOrderRequest request)
        {
            if (request.Items == null || !request.Items.Any())
                return Json(new { success = false, message = "Giỏ hàng trống" });

            try
            {
                decimal totalAmount = 0;
                var orderDetails = new List<OrderDetail>();

                // Kiểm tra và chuẩn bị chi tiết đơn hàng (KHÔNG trừ tồn kho vì đã trừ khi thêm vào giỏ)
                foreach (var item in request.Items)
                {
                    // Lấy ProductDetail với navigation properties để hiển thị
                    var details = await _productDetailRepo.FindAsync(
                        d => d.Id == item.ProductDetailId,
                        d => d.Product!,
                        d => d.Length!,
                        d => d.Color!,
                        d => d.Hardness!
                    );
                    var detail = details?.FirstOrDefault();

                    if (detail == null)
                        return Json(new { success = false, message = "Sản phẩm không tồn tại" });

                    decimal itemTotal = detail.Price * item.Quantity;
                    totalAmount += itemTotal;

                    orderDetails.Add(new OrderDetail
                    {
                        ProductDetailId = item.ProductDetailId,
                        ProductCodeAtOrder = detail.Product?.ProductCode,
                        NameAtOrder = detail.Product?.Name ?? "Sản phẩm",
                        Quantity = item.Quantity,
                        UnitPrice = detail.Price,
                        LengthAtOrder = detail.Length?.Name,
                        ColorAtOrder = detail.Color?.Name,
                        HardnessAtOrder = detail.Hardness?.Name,
                        TotalPrice = itemTotal
                    });
                }

                // Lấy phương thức thanh toán mặc định (Tiền mặt) cho đơn chờ
                var paymentMethods = await _paymentMethodRepo.GetAllAsync();
                var defaultPaymentMethod = paymentMethods?.FirstOrDefault();
                if (defaultPaymentMethod == null)
                    return Json(new { success = false, message = "Không tìm thấy phương thức thanh toán. Vui lòng thêm ít nhất một phương thức thanh toán." });

                // Xử lý CustomerId - nếu là khách vãng lai thì cần tạo Customer tạm hoặc dùng null
                Guid? customerId = null;
                if (request.CustomerId.HasValue && request.CustomerId.Value != Guid.Empty)
                {
                    // Kiểm tra Customer có tồn tại không
                    var existingCustomer = await _customerRepo.GetByIdAsync(request.CustomerId.Value);
                    if (existingCustomer != null)
                    {
                        customerId = request.CustomerId.Value;
                    }
                }

                // Tạo đơn hàng chờ POS (Status = -2, tồn kho đã được trừ khi thêm vào giỏ)
                var order = new Order
                {
                    CustomerId = customerId, // Null nếu là khách vãng lai
                    PaymentMethodId = defaultPaymentMethod.Id, // Sử dụng PTTT mặc định, sẽ cập nhật khi thanh toán
                    CreatedTime = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    FinalAmount = totalAmount,
                    Status = ORDER_STATUS_POS_PENDING, // Đơn chờ POS (đã trừ kho)
                    PhoneNumber = request.CustomerPhone ?? "Khách vãng lai",
                    ShippingAddress = request.CustomerAddress ?? "Bán tại quầy",
                    Note = $"[POS Pending] {request.CustomerName ?? "Khách vãng lai"} - Tạo lúc {DateTime.Now:HH:mm}",
                    ConfirmedByUserId = GetCurrentUserId()
                };

                await _orderRepo.AddAsync(order);

                // Thêm chi tiết đơn hàng
                foreach (var detail in orderDetails)
                {
                    detail.OrderId = order.Id;
                    await _orderDetailRepo.AddAsync(detail);
                }

                return Json(new
                {
                    success = true,
                    message = "Đã tạo đơn hàng chờ",
                    orderId = order.Id,
                    orderCode = $"POS-{order.Id.ToString().Substring(0, 8).ToUpper()}",
                    totalAmount = totalAmount,
                    customerName = request.CustomerName ?? "Khách vãng lai"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// API: Lấy danh sách đơn hàng chờ
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPendingOrders()
        {
            var currentUserId = GetCurrentUserId();
            var orders = await GetPendingOrdersAsync(currentUserId);

            return Json(orders.Select(o => new
            {
                id = o.Id,
                orderCode = $"POS-{o.Id.ToString().Substring(0, 8).ToUpper()}",
                customerName = o.Note?.Replace("[POS Pending] ", "").Split(" - ").FirstOrDefault() ?? "Khách vãng lai",
                customerPhone = o.PhoneNumber,
                totalAmount = o.TotalAmount,
                itemCount = o.Details?.Count ?? 0,
                createdTime = o.CreatedTime.ToLocalTime().ToString("HH:mm")
            }));
        }

        /// <summary>
        /// API: Lấy chi tiết đơn hàng chờ
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPendingOrderDetails(Guid orderId)
        {
            if (orderId == Guid.Empty)
                return Json(new { success = false, message = "ID đơn hàng không hợp lệ" });

            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null || order.Status != ORDER_STATUS_POS_PENDING)
                return Json(new { success = false, message = "Không tìm thấy đơn hàng chờ" });

            var details = await _orderDetailRepo.FindAsync(d => d.OrderId == orderId);

            return Json(new
            {
                success = true,
                order = new
                {
                    id = order.Id,
                    orderCode = $"POS-{order.Id.ToString().Substring(0, 8).ToUpper()}",
                    customerId = order.CustomerId,
                    customerPhone = order.PhoneNumber,
                    customerAddress = order.ShippingAddress,
                    customerName = order.Note?.Replace("[POS Pending] ", "").Split(" - ").FirstOrDefault() ?? "Khách vãng lai",
                    totalAmount = order.TotalAmount,
                    createdTime = order.CreatedTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                },
                items = details?.Select(d => new
                {
                    productDetailId = d.ProductDetailId,
                    productName = d.NameAtOrder,
                    variantText = $"{d.LengthAtOrder ?? ""} - {d.HardnessAtOrder ?? ""} - {d.ColorAtOrder ?? ""}".Trim(' ', '-'),
                    quantity = d.Quantity,
                    price = d.UnitPrice,
                    total = d.TotalPrice
                }).ToList()
            });
        }

        /// <summary>
        /// API: Hủy đơn hàng chờ - hoàn lại tồn kho
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CancelPendingOrder([FromBody] CancelPendingOrderRequest request)
        {
            if (request.OrderId == Guid.Empty)
                return Json(new { success = false, message = "ID đơn hàng không hợp lệ" });

            var order = await _orderRepo.GetByIdAsync(request.OrderId);
            if (order == null || order.Status != ORDER_STATUS_POS_PENDING)
                return Json(new { success = false, message = "Không tìm thấy đơn hàng chờ" });

            try
            {
                // Hoàn lại tồn kho
                var details = await _orderDetailRepo.FindAsync(d => d.OrderId == request.OrderId);
                foreach (var detail in details)
                {
                    var productDetail = await _productDetailRepo.GetByIdAsync(detail.ProductDetailId);
                    if (productDetail != null)
                    {
                        productDetail.StockQuantity += detail.Quantity;
                        await _productDetailRepo.UpdateAsync(productDetail);
                    }

                    // Xóa chi tiết đơn hàng
                    await _orderDetailRepo.DeleteAsync(detail.Id);
                }

                // Xóa đơn hàng
                await _orderRepo.DeleteAsync(request.OrderId);

                await _productDetailRepo.SaveChangesAsync();
                await _orderDetailRepo.SaveChangesAsync();
                await _orderRepo.SaveChangesAsync();

                return Json(new { success = true, message = "Đã hủy đơn hàng chờ và hoàn lại tồn kho" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// API: Thanh toán đơn hàng chờ
        /// </summary>
        [HttpPost]
        [RequirePermission("Sales", "CreateOrder")]
        public async Task<IActionResult> CompletePendingOrder([FromBody] CompletePendingOrderRequest request)
        {
            if (request.OrderId == Guid.Empty)
                return Json(new { success = false, message = "ID đơn hàng không hợp lệ" });

            if (request.PaymentMethodId == Guid.Empty)
                return Json(new { success = false, message = "Vui lòng chọn phương thức thanh toán" });

            var order = await _orderRepo.GetByIdAsync(request.OrderId);
            if (order == null || order.Status != ORDER_STATUS_POS_PENDING)
                return Json(new { success = false, message = "Không tìm thấy đơn hàng chờ" });

            try
            {
                // Cập nhật CustomerId nếu có
                if (request.CustomerId.HasValue && request.CustomerId.Value != Guid.Empty)
                {
                    order.CustomerId = request.CustomerId.Value;
                }

                // Áp dụng voucher nếu có
                decimal discount = 0;
                if (!string.IsNullOrWhiteSpace(request.VoucherCode))
                {
                    var vouchers = await _voucherRepo.FindAsync(v => v.Code == request.VoucherCode.Trim().ToUpper());
                    var voucher = vouchers?.FirstOrDefault();

                    if (voucher != null && (!voucher.EndDate.HasValue || DateTime.Now <= voucher.EndDate.Value))
                    {
                        if (!voucher.MaxUsage.HasValue || voucher.UsageCount < voucher.MaxUsage.Value)
                        {
                            order.VoucherId = voucher.Id;
                            if (voucher.DiscountPercentage.HasValue)
                            {
                                discount = order.TotalAmount * (decimal)voucher.DiscountPercentage.Value / 100;
                                if (voucher.MaxDiscountAmount.HasValue)
                                    discount = Math.Min(discount, voucher.MaxDiscountAmount.Value);
                            }
                            voucher.UsageCount++;
                            await _voucherRepo.UpdateAsync(voucher);
                        }
                    }
                }

                // Cập nhật đơn hàng
                order.PaymentMethodId = request.PaymentMethodId;
                order.DiscountAmount = discount > 0 ? discount : null;
                order.FinalAmount = order.TotalAmount - discount;
                order.Status = ORDER_STATUS_COMPLETED; // Hoàn thành
                order.PhoneNumber = request.CustomerPhone ?? order.PhoneNumber;
                order.ShippingAddress = request.CustomerAddress ?? order.ShippingAddress;
                order.Note = $"Bán hàng tại quầy (POS) - {(request.CustomerName ?? "Khách vãng lai")}";

                // Cập nhật SoldQuantity cho sản phẩm
                var details = await _orderDetailRepo.FindAsync(d => d.OrderId == request.OrderId);
                foreach (var detail in details)
                {
                    var productDetail = await _productDetailRepo.GetByIdAsync(detail.ProductDetailId);
                    if (productDetail != null)
                    {
                        productDetail.SoldQuantity += detail.Quantity;
                        await _productDetailRepo.UpdateAsync(productDetail);
                    }
                }

                await _orderRepo.UpdateAsync(order);
                await _orderRepo.SaveChangesAsync();
                await _productDetailRepo.SaveChangesAsync();
                await _voucherRepo.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Thanh toán thành công",
                    orderId = order.Id,
                    orderCode = $"POS-{order.Id.ToString().Substring(0, 8).ToUpper()}",
                    finalAmount = order.FinalAmount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// API: Thêm sản phẩm vào giỏ POS - TRỪA TỒN KHO NGAY LẬP TỨC
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddToCartPOS([FromBody] AddToCartPOSRequest request)
        {
            if (request.ProductDetailId == Guid.Empty)
                return Json(new { success = false, message = "Sản phẩm không hợp lệ" });

            if (request.Quantity <= 0)
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0" });

            try
            {
                var productDetail = await _productDetailRepo.GetByIdAsync(request.ProductDetailId);
                if (productDetail == null)
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });

                if (productDetail.StockQuantity < request.Quantity)
                    return Json(new { success = false, message = $"Không đủ hàng. Tồn kho còn: {productDetail.StockQuantity}" });

                // TRỪA TỒN KHO NGAY
                productDetail.StockQuantity -= request.Quantity;
                await _productDetailRepo.UpdateAsync(productDetail);

                return Json(new
                {
                    success = true,
                    message = $"Đã trừ {request.Quantity} sản phẩm khỏi tồn kho",
                    newStock = productDetail.StockQuantity
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// API: Xóa sản phẩm khỏi giỏ POS - HOÀN LẠI TỒN KHO
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RemoveFromCartPOS([FromBody] RemoveFromCartPOSRequest request)
        {
            if (request.ProductDetailId == Guid.Empty)
                return Json(new { success = false, message = "Sản phẩm không hợp lệ" });

            if (request.Quantity <= 0)
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0" });

            try
            {
                var productDetail = await _productDetailRepo.GetByIdAsync(request.ProductDetailId);
                if (productDetail == null)
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });

                // HOÀN LẠI TỒN KHO
                productDetail.StockQuantity += request.Quantity;
                await _productDetailRepo.UpdateAsync(productDetail);

                return Json(new
                {
                    success = true,
                    message = $"Đã hoàn lại {request.Quantity} sản phẩm vào tồn kho",
                    newStock = productDetail.StockQuantity
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// API: Cập nhật số lượng trong giỏ POS - điều chỉnh tồn kho theo chênh lệch
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCartQuantityPOS([FromBody] UpdateCartQuantityPOSRequest request)
        {
            if (request.ProductDetailId == Guid.Empty)
                return Json(new { success = false, message = "Sản phẩm không hợp lệ" });

            try
            {
                var productDetail = await _productDetailRepo.GetByIdAsync(request.ProductDetailId);
                if (productDetail == null)
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });

                int diff = request.NewQuantity - request.OldQuantity;

                if (diff > 0)
                {
                    // Tăng số lượng -> trừ thêm tồn kho
                    if (productDetail.StockQuantity < diff)
                        return Json(new { success = false, message = $"Không đủ hàng. Tồn kho còn: {productDetail.StockQuantity}" });

                    productDetail.StockQuantity -= diff;
                }
                else if (diff < 0)
                {
                    // Giảm số lượng -> hoàn lại tồn kho
                    productDetail.StockQuantity += Math.Abs(diff);
                }

                await _productDetailRepo.UpdateAsync(productDetail);

                return Json(new
                {
                    success = true,
                    message = diff > 0 ? $"Đã trừ thêm {diff} sản phẩm" : $"Đã hoàn lại {Math.Abs(diff)} sản phẩm",
                    newStock = productDetail.StockQuantity
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// API: Xóa toàn bộ giỏ hàng POS - hoàn lại tất cả tồn kho
        /// Hỗ trợ cả [FromBody] và sendBeacon
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ClearCartPOS([FromBody] ClearCartPOSRequest? request)
        {
            // Nếu request null hoặc rỗng (ví dụ sendBeacon có thể không bind), thử đọc từ Request.Body thủ công
            if (request == null || request.Items == null || !request.Items.Any())
            {
                try
                {
                    // Move stream position to beginning
                    Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
                    using var reader = new System.IO.StreamReader(Request.Body);
                    var body = await reader.ReadToEndAsync();
                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        // Deserialize with case-insensitive to accept blobs from navigator.sendBeacon
                        request = System.Text.Json.JsonSerializer.Deserialize<ClearCartPOSRequest>(body, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }
                catch { /* ignore parse errors */ }
            }

            if (request == null || request.Items == null || !request.Items.Any())
            {
                return Json(new { success = true, message = "Giỏ hàng đã trống" });
            }

            try
            {
                foreach (var item in request.Items)
                {
                    var productDetail = await _productDetailRepo.GetByIdAsync(item.ProductDetailId);
                    if (productDetail != null)
                    {
                        // If original stock provided, restore to that exact value to avoid double-adding
                        if (item.OriginalStock.HasValue)
                        {
                            productDetail.StockQuantity = item.OriginalStock.Value;
                        }
                        else
                        {
                            // Fallback: add back the quantity
                            productDetail.StockQuantity += item.Quantity;
                        }

                        await _productDetailRepo.UpdateAsync(productDetail);
                    }
                }

                await _productDetailRepo.SaveChangesAsync();

                return Json(new { success = true, message = "Đã hoàn lại tồn kho cho tất cả sản phẩm" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// API: Kiểm tra và áp dụng voucher
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ValidateVoucher([FromBody] ValidateVoucherRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.VoucherCode))
                return Json(new { success = false, message = "Nhập mã giảm giá" });

            var vouchers = await _voucherRepo.FindAsync(v => v.Code == request.VoucherCode.Trim().ToUpper());
            var voucher = vouchers?.FirstOrDefault();

            if (voucher == null)
                return Json(new { success = false, message = "Mã giảm giá không tồn tại" });

            if (voucher.EndDate.HasValue && DateTime.Now > voucher.EndDate.Value)
                return Json(new { success = false, message = "Mã voucher đã hết hạn" });

            if (voucher.MaxUsage.HasValue && voucher.UsageCount >= voucher.MaxUsage.Value)
                return Json(new { success = false, message = "Mã giảm giá đã hết lượt sử dụng" });

            if (voucher.MinOrderAmount.HasValue && request.TotalAmount < voucher.MinOrderAmount.Value)
                return Json(new { success = false, message = $"Đơn hàng phải tối thiểu {voucher.MinOrderAmount.Value:N0}₫" });

            decimal discount = 0;
            if (voucher.DiscountPercentage.HasValue)
            {
                discount = request.TotalAmount * (decimal)voucher.DiscountPercentage.Value / 100;
                if (voucher.MaxDiscountAmount.HasValue)
                {
                    discount = Math.Min(discount, voucher.MaxDiscountAmount.Value);
                }
            }

            return Json(new
            {
                success = true,
                voucherId = voucher.Id,
                discount = discount,
                finalAmount = request.TotalAmount - discount
            });
        }

        /// <summary>
        /// Tạo đơn hàng tại quầy (thanh toán ngay - tồn kho đã được trừ khi thêm vào giỏ)
        /// </summary>
        [HttpPost]
        [RequirePermission("Sales", "CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] SalesOrderModel model)
        {
            if (model == null || model.Items == null || !model.Items.Any())
                return Json(new { success = false, message = "Giỏ hàng trống" });

            try
            {
                // Kiểm tra phương thức thanh toán
                if (model.PaymentMethodId == Guid.Empty)
                    return Json(new { success = false, message = "Vui lòng chọn phương thức thanh toán" });

                // Tìm hoặc xử lý khách hàng
                Guid? customerId = null;
                string customerPhone = model.CustomerPhone ?? "Khách vãng lai";
                string customerAddress = model.ShippingAddress ?? "Bán tại quầy";

                if (!string.IsNullOrWhiteSpace(model.CustomerPhone) && model.CustomerPhone != "Khách vãng lai")
                {
                    var customers = await _customerService.GetAllAsyncWithUser();
                    var customer = customers?.FirstOrDefault(c =>
                        c.User?.PhoneNumber != null &&
                        c.User.PhoneNumber.Trim() == model.CustomerPhone.Trim());

                    if (customer != null)
                    {
                        customerId = customer.Id;
                        customerAddress = model.ShippingAddress ?? customer.Address ?? "Bán tại quầy";
                    }
                }

                // Tính tổng tiền và chuẩn bị chi tiết đơn hàng (KHÔNG trừ tồn kho vì đã trừ khi thêm vào giỏ)
                decimal totalAmount = 0;
                var orderDetails = new List<OrderDetail>();

                foreach (var item in model.Items)
                {
                    var details = await _productDetailRepo.FindAsync(
                        d => d.Id == item.ProductDetailId,
                        d => d.Product!,
                        d => d.Length!,
                        d => d.Color!,
                        d => d.Hardness!
                    );
                    var detail = details?.FirstOrDefault();

                    if (detail == null)
                        return Json(new { success = false, message = "Sản phẩm không tồn tại" });

                    decimal itemTotal = detail.Price * item.Quantity;
                    totalAmount += itemTotal;

                    orderDetails.Add(new OrderDetail
                    {
                        ProductDetailId = item.ProductDetailId,
                        ProductCodeAtOrder = detail.Product?.ProductCode,
                        NameAtOrder = detail.Product?.Name ?? "Sản phẩm",
                        Quantity = item.Quantity,
                        UnitPrice = detail.Price,
                        LengthAtOrder = detail.Length?.Name,
                        ColorAtOrder = detail.Color?.Name,
                        HardnessAtOrder = detail.Hardness?.Name,
                        TotalPrice = itemTotal
                    });
                }

                // Áp dụng voucher
                decimal discount = 0;
                Guid? voucherId = null;

                if (!string.IsNullOrWhiteSpace(model.VoucherCode))
                {
                    var vouchers = await _voucherRepo.FindAsync(v => v.Code == model.VoucherCode.Trim().ToUpper());
                    var voucher = vouchers?.FirstOrDefault();

                    if (voucher != null && (!voucher.EndDate.HasValue || DateTime.Now <= voucher.EndDate.Value))
                    {
                        if (!voucher.MaxUsage.HasValue || voucher.UsageCount < voucher.MaxUsage.Value)
                        {
                            voucherId = voucher.Id;
                            if (voucher.DiscountPercentage.HasValue)
                            {
                                discount = totalAmount * (decimal)voucher.DiscountPercentage.Value / 100;
                                if (voucher.MaxDiscountAmount.HasValue)
                                    discount = Math.Min(discount, voucher.MaxDiscountAmount.Value);
                            }
                            voucher.UsageCount++;
                            await _voucherRepo.UpdateAsync(voucher);
                        }
                    }
                }

                decimal finalAmount = totalAmount - discount;

                // Tạo đơn hàng hoàn thành
                var order = new Order
                {
                    CustomerId = customerId, // Null nếu là khách vãng lai
                    VoucherId = voucherId,
                    PaymentMethodId = model.PaymentMethodId,
                    CreatedTime = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    DiscountAmount = discount > 0 ? discount : null,
                    FinalAmount = finalAmount,
                    Status = ORDER_STATUS_COMPLETED,
                    PhoneNumber = customerPhone,
                    ShippingAddress = customerAddress,
                    Note = string.IsNullOrWhiteSpace(model.Note) ? "Bán hàng tại quầy (POS)" : model.Note,
                    ConfirmedByUserId = GetCurrentUserId()
                };

                await _orderRepo.AddAsync(order);

                // Chỉ cập nhật SoldQuantity (tồn kho đã được trừ khi thêm vào giỏ)
                foreach (var item in model.Items)
                {
                    var detail = await _productDetailRepo.GetByIdAsync(item.ProductDetailId);
                    if (detail != null)
                    {
                        detail.SoldQuantity += item.Quantity;
                        await _productDetailRepo.UpdateAsync(detail);
                    }
                }

                foreach (var od in orderDetails)
                {
                    od.OrderId = order.Id;
                    // ensure ProductCodeAtOrder preserved (already set when built)
                    await _orderDetailRepo.AddAsync(od);
                }

                await _orderRepo.SaveChangesAsync();
                await _orderDetailRepo.SaveChangesAsync();
                await _productDetailRepo.SaveChangesAsync();
                await _voucherRepo.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Tạo đơn hàng thành công",
                    orderId = order.Id,
                    orderCode = $"POS-{order.Id.ToString().Substring(0, 8).ToUpper()}",
                    finalAmount = order.FinalAmount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        /// <summary>
        /// In hóa đơn
        /// </summary>
        [HttpGet]
        [RequirePermission("Sales", "PrintReceipt")]
        public async Task<IActionResult> PrintReceipt(Guid orderId)
        {
            if (orderId == Guid.Empty) return BadRequest();

            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return NotFound();

            // Load Customer info
            if (order.CustomerId.HasValue && order.CustomerId.Value != Guid.Empty)
            {
                var customer = await _customerRepo.GetByIdAsync(order.CustomerId.Value);
                if (customer != null)
                {
                    var user = await _userRepo.GetByIdAsync(customer.UserId);
                    customer.User = user;
                    order.Customer = customer;
                }
            }

            // Load PaymentMethod
            if (order.PaymentMethodId != Guid.Empty)
            {
                order.PaymentMethod = await _paymentMethodRepo.GetByIdAsync(order.PaymentMethodId);
            }

            var details = await _orderDetailRepo.FindAsync(d => d.OrderId == orderId);
            ViewBag.Details = details?.ToList() ?? new List<OrderDetail>();
            ViewBag.OrderCode = $"POS-{orderId.ToString().Substring(0, 8).ToUpper()}";

            return View(order);
        }

        /// <summary>
        /// Lịch sử bán hàng tại quầy
        /// </summary>
        [HttpGet]
        [RequirePermission("Sales", "History")]
        public async Task<IActionResult> History(int days = 30)
        {
            var from = DateTime.UtcNow.Date.AddDays(-days);

            var allOrders = await _orderRepo.GetAllAsync();
            var posOrders = allOrders?
                .Where(o => o.CreatedTime >= from &&
                           o.Status == ORDER_STATUS_COMPLETED &&
                           (o.ConfirmedByUserId != null && o.ConfirmedByUserId != Guid.Empty) &&
                           // Only include orders created by POS flow. CreateOrder and CompletePendingOrder set Note to include "POS" or "Bán hàng tại quầy"
                           ((o.Note != null && (o.Note.IndexOf("POS", StringComparison.OrdinalIgnoreCase) >= 0)) ||
                            (o.Note != null && (o.Note.IndexOf("Bán hàng tại quầy", StringComparison.OrdinalIgnoreCase) >= 0))))
                .OrderByDescending(o => o.CreatedTime)
                .ToList() ?? new List<Order>();

            foreach (var order in posOrders)
            {
                if (order.PaymentMethodId != Guid.Empty)
                {
                    order.PaymentMethod = await _paymentMethodRepo.GetByIdAsync(order.PaymentMethodId);
                }
            }

            ViewBag.Days = days;
            return View(posOrders);
        }

        #region Private Helpers

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        private async Task<List<Order>> GetPendingOrdersAsync(Guid userId)
        {
            var allOrders = await _orderRepo.GetAllAsync();
            var pendingOrders = allOrders?
                .Where(o => o.Status == ORDER_STATUS_POS_PENDING && o.ConfirmedByUserId == userId)
                .OrderByDescending(o => o.CreatedTime)
                .ToList() ?? new List<Order>();

            // Load order details
            foreach (var order in pendingOrders)
            {
                var details = await _orderDetailRepo.FindAsync(d => d.OrderId == order.Id);
                order.Details = details?.ToList();
            }

            return pendingOrders;
        }

        #endregion
    }

    #region Request Models

    public class ValidateVoucherRequest
    {
        public string VoucherCode { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }

    public class SalesOrderModel
    {
        public string? CustomerPhone { get; set; }
        public string? ShippingAddress { get; set; }
        public string? VoucherCode { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string? Note { get; set; }
        public List<SalesOrderItem> Items { get; set; } = new();
    }

    public class SalesOrderItem
    {
        public Guid ProductDetailId { get; set; }
        public int Quantity { get; set; }
        // optional - used by ClearCartPOS beacon to restore exact original stock
        public int? OriginalStock { get; set; }
    }

    public class QuickCreateCustomerRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class CreatePendingOrderRequest
    {
        public Guid? CustomerId { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public List<SalesOrderItem> Items { get; set; } = new();
    }

    public class CancelPendingOrderRequest
    {
        public Guid OrderId { get; set; }
    }

    public class CompletePendingOrderRequest
    {
        public Guid OrderId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? VoucherCode { get; set; }
        public Guid PaymentMethodId { get; set; }
    }

    public class AddToCartPOSRequest
    {
        public Guid ProductDetailId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddToCartForCustomerRequest
    {
        public Guid ProductDetailId { get; set; }
        public int Quantity { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerName { get; set; }
    }

    public class RemoveFromCartPOSRequest
    {
        public Guid ProductDetailId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartQuantityPOSRequest
    {
        public Guid ProductDetailId { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
    }

    public class ClearCartPOSRequest
    {
        public List<SalesOrderItem> Items { get; set; } = new();
    }

    #endregion
}
