using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class OrderDetailWebService : OrderDetailService
    {
        public OrderDetailWebService(IGenericRepository<OrderDetail> repository) : base(repository)
        {
        }
    }
}
