using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class ProductImageService : GenericService<ProductImage>
    {
        public ProductImageService(IGenericRepository<ProductImage> repository) : base(repository)
        {
        }
    }
}