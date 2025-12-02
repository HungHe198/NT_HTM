using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class ColorWebService : ColorService
    {
        public ColorWebService(IGenericRepository<Color> repository) : base(repository) { }

        public Task<IEnumerable<Color>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();
            Expression<Func<Color, bool>> predicate = c => c.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
}