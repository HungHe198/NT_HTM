using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NT.WEB.Services
{
    public class CartDetailWebService : CartDetailService
    {
        public CartDetailWebService(IGenericRepository<CartDetail> repository) : base(repository)
        {
        }

        public Task<IEnumerable<CartDetail>> FindWithIncludesAsync(Expression<Func<CartDetail, bool>> predicate, params Expression<Func<CartDetail, object>>[] includes)
        {
            return _repository.FindAsync(predicate, includes);
        }
    }
}
