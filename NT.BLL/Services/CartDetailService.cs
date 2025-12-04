using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class CartDetailService : GenericService<CartDetail>
    {
        public CartDetailService(IGenericRepository<CartDetail> repository) : base(repository)
        {
        }
    }
}
