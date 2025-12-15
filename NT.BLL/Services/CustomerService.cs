using NT.BLL.Interfaces;
using NT.SHARED.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NT.BLL.Services
{
    public class CustomerService : GenericService<Customer>
    {
        public CustomerService(IGenericRepository<Customer> repository) : base(repository)
        {
        }

        /// <summary>
        /// Lấy tất cả khách hàng cùng với thông tin User
        /// </summary>
        public async Task<IEnumerable<Customer>> GetAllAsyncWithUser()
        {
            return await _repository.FindAsync(
                c => true,
                c => c.User!
            );
        }
    }
}
