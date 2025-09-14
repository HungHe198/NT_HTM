using NT.BLL.Interface;
using NT.SHARED.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NT.BLL.Services
{
    /// <summary>
    /// Service logic cho nghi?p v? qu?n lý s?n ph?m.
    /// </summary>
    public class ProductService : IProductService, ISearchByNameService<Product>
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _productRepository.GetAllAsync();

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id s?n ph?m không h?p l?", nameof(id));
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            // Validate các tr??ng b?t bu?c
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Tên s?n ph?m không ???c ?? tr?ng");
            // ...validate các tr??ng khác n?u c?n
            return await _productRepository.AddAsync(product);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (product.Id == Guid.Empty)
                throw new ArgumentException("Id s?n ph?m không h?p l?");
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id s?n ph?m không h?p l?", nameof(id));
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên tìm ki?m không ???c ?? tr?ng", nameof(name));
            return await _productRepository.SearchByNameAsync(name);
        }
    }
}