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
            var user = await _userRepo.GetByIdAsync(userId) ?? throw new InvalidOperationException("User kh�ng t?n t?i");
            // Kh�ng th? g�n tr?c ti?p v� setter private
            throw new NotSupportedException("Kh�ng th? thay ??i tr?ng th�i User v� thu?c t�nh Status c� setter private. H�y th�m ph??ng th?c domain ?? c?p nh?t tr?ng th�i.");
        }
    }
}
