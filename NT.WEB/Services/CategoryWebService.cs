using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    // Inherit BLL implementation and add web-specific helpers
    public class CategoryWebService : CategoryService
    {
        public CategoryWebService(IGenericRepository<Category> repository)
            : base(repository)
        {
        }

        // Example helper used by Razor Pages: search by partial name
        public Task<IEnumerable<Category>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();

            Expression<Func<Category, bool>> predicate = c => c.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
}