using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class SurfaceFinishWebService : SurfaceFinishService
    {
        public SurfaceFinishWebService(IGenericRepository<SurfaceFinish> repository) : base(repository) { }

        public Task<IEnumerable<SurfaceFinish>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();
            Expression<Func<SurfaceFinish, bool>> predicate = s => s.Name.Contains(partialName);
            return _repository.FindAsync(predicate);
        }
    }
}