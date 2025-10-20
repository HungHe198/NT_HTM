using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.BLL.Interfaces;

namespace NT.BLL.Services
{
    /// <summary>
    /// Tri?n khai m?c ??nh cho IGenericService s? d?ng IGenericRepository.
    /// T?t c? CRUD ch?m DB ch? ?i qua repository (DAL).
    /// </summary>
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        protected readonly IGenericRepository<TEntity> _repository;

        public GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<IEnumerable<TEntity>> GetAllAsync() => _repository.GetAllAsync();

        public Task<TEntity?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            await _repository.AddAsync(entity);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            await _repository.UpdateAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
