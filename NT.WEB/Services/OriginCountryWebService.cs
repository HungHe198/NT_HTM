using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class OriginCountryWebService : OriginCountryService
    {
        public OriginCountryWebService(IGenericRepository<OriginCountry> repository) : base(repository) { }

        public Task<IEnumerable<OriginCountry>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();
            Expression<Func<OriginCountry, bool>> predicate = o => o.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
}