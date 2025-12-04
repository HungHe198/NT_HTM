using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class CustomerService : GenericService<Customer>
    {
        public CustomerService(IGenericRepository<Customer> repository) : base(repository)
        {
        }
    }
}
