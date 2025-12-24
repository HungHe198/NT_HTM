using NT.BLL.Interfaces;
using NT.SHARED.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NT.BLL.Services
{
    public class ProductCategoryService : GenericService<ProductCategory>
    {
        public ProductCategoryService(IGenericRepository<ProductCategory> repository) : base(repository)
        {
        }

        /// <summary>
        /// Delete a ProductCategory by composite keys.
        /// Returns true if deleted, false if not found.
        /// </summary>
        public async Task<bool> DeleteByIdsAsync(Guid categoryId, Guid productId)
        {
            if (categoryId == Guid.Empty || productId == Guid.Empty) throw new ArgumentException("categoryId or productId is not valid");

            var items = await _repository.FindAsync(pc => pc.CategoryId == categoryId && pc.ProductId == productId);
            var item = items?.FirstOrDefault();
            if (item == null) return false;

            await _repository.DeleteEntityAsync(item);
            return true;
        }
    }
}
