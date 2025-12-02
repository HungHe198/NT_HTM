using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class BrandWebService : BrandService
    {
        public BrandWebService(IGenericRepository<Brand> repository) : base(repository)
        {
        }
        public Task<IEnumerable<Brand>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();
            System.Linq.Expressions.Expression<System.Func<Brand, bool>> predicate = b => b.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
    
    
}
