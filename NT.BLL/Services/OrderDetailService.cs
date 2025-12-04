using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class OrderDetailService : GenericService<OrderDetail>
    {
        public OrderDetailService(IGenericRepository<OrderDetail> repository) : base(repository)
        {
        }
    }
}
