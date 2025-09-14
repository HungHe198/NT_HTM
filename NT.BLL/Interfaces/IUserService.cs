using NT.SHARED.Models;

namespace NT.BLL.Interface
{
    /// <summary>
    /// D?ch v? qu?n lý ng??i dùng.
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Role>> GetRolesAsync(Guid userId);
    }
}