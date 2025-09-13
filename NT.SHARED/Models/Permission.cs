using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Models
{
    public class Permission
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100)]
        public string Name { get; private set; } = null!; // Tên quyền (VD: Xem sản phẩm)
        [Required, MaxLength(50)]
        public string Code { get; private set; } = null!; // Mã quyền (VD: PRODUCT_VIEW)
        [Required, MaxLength(50)]
        public string Resource { get; private set; } = null!; // Đối tượng (VD: Product)
        [Required, MaxLength(50)]
        public string Action { get; private set; } = null!; // Hành động (VD: View)
        [Required, MaxLength(200)]
        public string Endpoint { get; private set; } = null!; // API endpoint (VD: /api/products)
        [Required, MaxLength(10)]
        public string HttpMethod { get; private set; } = null!; // GET, POST
        [Required, MaxLength(200)]
        public string ViewPath { get; private set; } = null!; // /Views/Product/Index.cshtml
        [MaxLength(100)]
        public string? Description { get; private set; } // Mô tả

        // Private constructor for EF
        private Permission() { }

        // Public static factory
        public static Permission Create(string name, string code, string resource, string action, string endpoint, string httpMethod, string viewPath, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Không để trống tên của quyền", nameof(name));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Không được để trống mã của quyền", nameof(code));
            if (string.IsNullOrWhiteSpace(resource)) throw new ArgumentException("Không được để trống đối tượng", nameof(resource));
            if (string.IsNullOrWhiteSpace(action)) throw new ArgumentException("Vui lòng chọn các thao tác hợp lệ", nameof(action));
            if (string.IsNullOrWhiteSpace(endpoint)) throw new ArgumentException("URL truy cập không hợp lệ", nameof(endpoint));
            if (string.IsNullOrWhiteSpace(httpMethod)) throw new ArgumentException("Phương thức HTTP không được để trống", nameof(httpMethod));
            if (string.IsNullOrWhiteSpace(viewPath)) throw new ArgumentException("Đường dẫn giao diện không được để trống", nameof(viewPath));
            return new Permission
            {
                Name = name,
                Code = code,
                Resource = resource,
                Action = action,
                Endpoint = endpoint,
                HttpMethod = httpMethod,
                ViewPath = viewPath,
                Description = description
            };
        }

        // Navigation
        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();
    }
}