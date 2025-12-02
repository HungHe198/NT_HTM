using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class ProductImageWebService : ProductImageService
    {
        public ProductImageWebService(IGenericRepository<ProductImage> repository) : base(repository)
        {
        }

        public Task<IEnumerable<ProductImage>> GetByProductDetailIdAsync(Guid productDetailId)
        {
            if (productDetailId == Guid.Empty) return Task.FromResult<IEnumerable<ProductImage>>(new List<ProductImage>());
            Expression<Func<ProductImage, bool>> predicate = pi => pi.ProductDetailId == productDetailId;
            return _repository.FindAsync(predicate);
        }

        public Task<IEnumerable<ProductImage>> SearchByUrlAsync(string partialUrl)
        {
            if (string.IsNullOrWhiteSpace(partialUrl)) return _repository.GetAllAsync();
            Expression<Func<ProductImage, bool>> predicate = pi => pi.ImageUrl.Contains(partialUrl);
            return _repository.FindAsync(predicate);
        }
    }
}