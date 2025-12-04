using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class ProductCategoryService : GenericService<ProductCategory>
    {
        public ProductCategoryService(IGenericRepository<ProductCategory> repository) : base(repository)
        {
        }
    }
}
