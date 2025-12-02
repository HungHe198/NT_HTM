using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class ProductDetailService : GenericService<ProductDetail>
    {
        public ProductDetailService(IGenericRepository<ProductDetail> repository) : base(repository)
        {
        }
    }
}