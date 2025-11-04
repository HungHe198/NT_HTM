using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NT.BLL.Interfaces;

namespace NT.BLL.Services
{
    /// <summary>
    /// Triển khai mặc định cho IGenericService sử dụng IGenericRepository.
    /// Tất cả CRUD chuyển xuống repository (DAL).
    /// </summary>
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        protected readonly IGenericRepository<TEntity> _repository;

        public GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Lấy tất cả entity.
        /// </summary>
        public Task<IEnumerable<TEntity>> GetAllAsync() => _repository.GetAllAsync();

        /// <summary>
        /// Lấy entity theo id.
        /// </summary>
        /// <exception cref="ArgumentException">Nếu id == Guid.Empty</exception>
        public Task<TEntity?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id không hợp lệ", nameof(id));
            return _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Thêm entity mới.
        /// </summary>
        /// <exception cref="ArgumentNullException">Nếu entity là null</exception>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            await _repository.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Cập nhật entity.
        /// </summary>
        /// <exception cref="ArgumentNullException">Nếu entity là null</exception>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            await _repository.UpdateAsync(entity);
            return entity;
        }

        /// <summary>
        /// Xóa entity theo id.
        /// Trả về true nếu xóa thành công, false nếu có lỗi (không throw).
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id không hợp lệ", nameof(id));

            try
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            catch
            {
                // swallow exception to keep contract of returning bool.
                // If caller needs exception detail, use repository directly or change contract.
                return false;
            }
        }

        /// <summary>
        /// Lưu tất cả thay đổi pending xuống DB (delegates to repository).
        /// </summary>
        public Task SaveChangesAsync() => _repository.SaveChangesAsync();

        /// <summary>
        /// Tìm kiếm theo biểu thức predicate (delegates to repository).
        /// </summary>
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _repository.FindAsync(predicate);
        }
    }
}
