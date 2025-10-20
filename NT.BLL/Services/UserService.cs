using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;

        public UserService(
            IGenericRepository<User> userRepo,
            IGenericRepository<Role> roleRepo) : base(userRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        public Task<IEnumerable<Role>> GetRolesAsync(Guid userId)
        {
            return _roleRepo.GetAllAsync();
        }

        public async Task<User> ChangeStatusAsync(Guid userId, bool newStatus)
        {
            var user = await _userRepo.GetByIdAsync(userId) ?? throw new InvalidOperationException("User không t?n t?i");
            // Không th? gán tr?c ti?p vì setter private
            throw new NotSupportedException("Không th? thay ??i tr?ng thái User vì thu?c tính Status có setter private. Hãy thêm ph??ng th?c domain ?? c?p nh?t tr?ng thái.");
        }
    }
}
