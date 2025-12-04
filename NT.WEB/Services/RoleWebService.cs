using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class RoleWebService : RoleService
    {
        public RoleWebService(IGenericRepository<Role> repository, IGenericRepository<Permission> permissionRepo) : base(repository, permissionRepo)
        {
        }
    }
}
