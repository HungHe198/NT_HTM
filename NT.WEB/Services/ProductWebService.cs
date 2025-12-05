using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class ProductWebService : ProductService, ISearchByNameService<Product>
    {
        public ProductWebService(IGenericRepository<Product> repository) : base(repository)
        {
        }
        public Task<IEnumerable<Product>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();

            var term = partialName.Trim().ToLowerInvariant();
            Expression<Func<Product, bool>> predicate = p =>
                (p.Name != null && p.Name.ToLower().Contains(term)) ||
                (p.ProductCode != null && p.ProductCode.ToLower().Contains(term));

            return _repository.FindAsync(predicate);
        }
    }
}
