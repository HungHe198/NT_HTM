using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class ProductWebService : ProductService
    {
        public ProductWebService(IGenericRepository<Product> repository) : base(repository)
        {
        }
        public Task<IEnumerable<Product>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();
            System.Linq.Expressions.Expression<System.Func<Product, bool>> predicate = p => p.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
}
