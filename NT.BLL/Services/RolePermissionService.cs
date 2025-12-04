using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class RolePermissionService : GenericService<RolePermission>
    {
        public RolePermissionService(IGenericRepository<RolePermission> repository) : base(repository)
        {
        }
    }
}
