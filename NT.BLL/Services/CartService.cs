using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class CartService : GenericService<Cart>
    {
        public CartService(IGenericRepository<Cart> repository) : base(repository)
        {
        }
    }
}
