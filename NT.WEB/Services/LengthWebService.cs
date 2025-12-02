using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class LengthWebService : LengthService
    {
        public LengthWebService(IGenericRepository<Length> repository) : base(repository) { }

        public Task<IEnumerable<Length>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();
            Expression<Func<Length, bool>> predicate = l => l.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
}