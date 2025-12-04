using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class CartDetailWebService : CartDetailService
    {
        public CartDetailWebService(IGenericRepository<CartDetail> repository) : base(repository)
        {
        }
    }
}
