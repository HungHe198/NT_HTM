using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class HardnessWebService : HardnessService
    {
        public HardnessWebService(IGenericRepository<Hardness> repository) : base(repository) { }

        public Task<IEnumerable<Hardness>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();
            Expression<Func<Hardness, bool>> predicate = h => h.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
}