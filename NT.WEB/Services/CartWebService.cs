using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class CartWebService : CartService
    {
        public CartWebService(IGenericRepository<Cart> repository) : base(repository)
        {
        }
    }
}
