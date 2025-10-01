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
        [Required, MaxLength(150)]
        public string Code { get; private set; } = null!; // Ví dụ: "PRODUCT_CREATE"
        [Required, MaxLength(200)]
        public string Description { get; private set; } = null!;
        [Required, MaxLength(100)]
        public string Resource { get; private set; } = null!; // Endpoint / Controller
        [Required, MaxLength(50)]
        public string Action { get; private set; } = null!; // GET, POST, PUT, DELETE

        private Permission() { }

        public static Permission Create(string code, string description, string resource, string action)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code không hợp lệ");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description không hợp lệ");
            if (string.IsNullOrWhiteSpace(resource)) throw new ArgumentException("Resource không hợp lệ");
            if (string.IsNullOrWhiteSpace(action)) throw new ArgumentException("Action không hợp lệ");
            return new Permission { Code = code.Trim(), Description = description.Trim(), Resource = resource.Trim(), Action = action.Trim() };
        }

        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();
    }
}