using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class ProductService : GenericService<Product>, IProductService, ISearchByNameService<Product>
    {
        public ProductService(IGenericRepository<Product> repository) : base(repository)
        {

        }       
        public Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return _repository.GetAllAsync();

            Expression<Func<Product, bool>> predicate = p => p.Name.Contains(name);
            return _repository.FindAsync(predicate);
        }
    }
}
    