using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT.SHARED.Models
{
    public class Permission
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required, MaxLength(100), Display(Name = "Mã quyền hạn")]
        public string Code { get; private set; } = null!;
        [MaxLength(250), Display(Name = "Mô tả")]
        public string? Description { get; private set; }
        [MaxLength(100), Display(Name = "Tài nguyên được truy cập(Resource)")]
        public string? Resource { get; private set; }
        [MaxLength(100), Display(Name = "Hành động được phép truy cập(Action)")]
        public string? Action { get; private set; }
        [MaxLength(10), Display(Name = "Phương thức HTTP(Method)")]
        public string? Method { get; private set; }

        private Permission() { }
        public static Permission Create(string code, string? description = null, string? resource = null, string? action = null, string? method = null)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Vui lòng nhập mã quyền");
            return new Permission
            {
                Code = code.Trim(),
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                Resource = string.IsNullOrWhiteSpace(resource) ? null : resource.Trim(),
                Action = string.IsNullOrWhiteSpace(action) ? null : action.Trim(),
                Method = string.IsNullOrWhiteSpace(method) ? null : method.Trim()
            };
        }

        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();
    }
}
