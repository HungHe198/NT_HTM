using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class ProductDetailWebService : ProductDetailService
    {
        public ProductDetailWebService(IGenericRepository<ProductDetail> repository) : base(repository)
        {
        }

        public Task<IEnumerable<ProductDetail>> GetByProductIdAsync(Guid productId)
        {
            if (productId == Guid.Empty) return Task.FromResult<IEnumerable<ProductDetail>>(new List<ProductDetail>());
            Expression<Func<ProductDetail, bool>> predicate = pd => pd.ProductId == productId;
            return _repository.FindAsync(predicate);
        }

        public Task<IEnumerable<ProductDetail>> GetWithLookupsByProductIdAsync(Guid productId)
        {
            if (productId == Guid.Empty) return Task.FromResult<IEnumerable<ProductDetail>>(new List<ProductDetail>());
            Expression<Func<ProductDetail, bool>> predicate = pd => pd.ProductId == productId;
            return _repository.FindAsync(
                predicate,
                pd => pd.Length,
                pd => pd.SurfaceFinish,
                pd => pd.Hardness,
                pd => pd.Elasticity,
                pd => pd.OriginCountry,
                pd => pd.Color
            );
        }
    }
}