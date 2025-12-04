using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class ProductCategoryWebService : ProductCategoryService
    {
        public ProductCategoryWebService(IGenericRepository<ProductCategory> repository) : base(repository)
        {
        }
    }
}
