using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class CustomerWebService : CustomerService
    {
        public CustomerWebService(IGenericRepository<Customer> repository) : base(repository)
        {
        }
    }
}
