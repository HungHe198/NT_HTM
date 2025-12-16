using Microsoft.EntityFrameworkCore;
using NT.BLL.Interfaces;
using NT.DAL.ContextFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NT.DAL.Repositories
{
    /// <summary>
    /// Implementation generic cho IGenericRepository, hỗ trợ CRUD cơ bản cho mọi entity.
    /// Sử dụng EF Core với NTAppDbContext để truy cập dữ liệu.
    /// </summary>
    /// <typeparam name="H">Loại entity (phải là class) đại diện cho bảng trong cơ sở dữ liệu.</typeparam>
    public class GenericRepository<H> : IGenericRepository<H> where H : class
    {
        protected readonly NTAppDbContext _context;
        protected readonly DbSet<H> _dbSet;

        /// <summary>
        /// Khởi tạo GenericRepository với DbContext.
        /// </summary>
        /// <param name="context">NTAppDbContext để truy cập cơ sở dữ liệu.</param>
        /// <exception cref="ArgumentNullException">Ném nếu context là null.</exception>
        public GenericRepository(NTAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<H>();
        }

        /// <summary>
        /// Lấy tất cả các bản ghi từ bảng tương ứng với entity H.
        /// </summary>
        public async Task<IEnumerable<H>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Lấy một bản ghi duy nhất dựa trên ID.
        /// </summary>
        public async Task<H?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("ID không hợp lệ", nameof(id));
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Thêm một entity mới vào bảng tương ứng.
        /// </summary>
        public async Task AddAsync(H entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật một entity hiện có trong bảng.
        /// </summary>
        public async Task UpdateAsync(H entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Xóa một entity dựa trên ID.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id) ?? throw new NotFoundException("Không tìm thấy entity để xóa");
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lưu tất cả các thay đổi đang pending vào cơ sở dữ liệu.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Tìm kiếm các entity theo điều kiện cho trước.
        /// </summary>
        /// <param name="predicate">Điều kiện để xác định các bản ghi cần tìm.</param>
        /// <returns>Danh sách các entity thỏa mãn điều kiện.</returns>
        public async Task<IEnumerable<H>> FindAsync(Expression<Func<H, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Tìm kiếm với Include các navigation properties.
        /// </summary>
        public async Task<IEnumerable<H>> FindAsync(Expression<Func<H, bool>> predicate, params Expression<Func<H, object>>[] includes)
        {
            IQueryable<H> query = _dbSet;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Xóa entity theo điều kiện (dùng cho composite key hoặc xóa nhiều bản ghi).
        /// </summary>
        public async Task<int> DeleteWhereAsync(Expression<Func<H, bool>> predicate)
        {
            var entities = await _dbSet.Where(predicate).ToListAsync();
            if (entities.Any())
            {
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            return entities.Count;
        }

        /// <summary>
        /// Xóa một entity cụ thể (dùng cho entity có composite key).
        /// </summary>
        public async Task DeleteEntityAsync(H entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Ngoại lệ tùy chỉnh khi không tìm thấy entity.
        /// </summary>
        private class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }
    }
}
