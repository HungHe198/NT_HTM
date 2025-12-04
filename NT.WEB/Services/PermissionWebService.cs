using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class PermissionWebService : PermissionService
    {
        public PermissionWebService(IGenericRepository<Permission> repository) : base(repository)
        {
        }
    }
}
