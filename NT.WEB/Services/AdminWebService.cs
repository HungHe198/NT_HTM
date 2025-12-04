using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class AdminWebService : AdminService
    {
        public AdminWebService(IGenericRepository<Admin> repository) : base(repository)
        {
        }
    }
}
