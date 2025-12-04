using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class AdminService : GenericService<Admin>
    {
        public AdminService(IGenericRepository<Admin> repository) : base(repository)
        {
        }
    }
}
