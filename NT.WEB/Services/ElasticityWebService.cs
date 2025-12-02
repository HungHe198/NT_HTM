using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class ElasticityWebService : ElasticityService
    {
        public ElasticityWebService(IGenericRepository<Elasticity> repository) : base(repository) { }

        public Task<IEnumerable<Elasticity>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();
            Expression<Func<Elasticity, bool>> predicate = e => e.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
}