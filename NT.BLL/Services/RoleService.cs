using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class RoleService : GenericService<Role>, IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<Permission> _permissionRepo;

        public RoleService(
            IGenericRepository<Role> roleRepo,
            IGenericRepository<Permission> permissionRepo) : base(roleRepo)
        {
            _roleRepo = roleRepo;
            _permissionRepo = permissionRepo;
        }

        public Task<IEnumerable<Permission>> GetPermissionsAsync(Guid roleId)
        {
            // ? m?c t?i thi?u, l?y t?t c? permission, th?c t? nên có b?ng RolePermission riêng.
            return _permissionRepo.GetAllAsync();
        }
    }
}
