
using LoginSignup.DTOs;
using LoginSignup.Models;
using LoginSignup.Repositories.Interfaces;
using LoginSignup.Services.Interfaces;

namespace LoginSignup.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepo;

        public AdminService(IUserRepository userRepo) => _userRepo = userRepo;

        public async Task<List<User>> GetAllUsersAsync(string currentAdminEmail)
        {
            var admin = await _userRepo.GetByEmailAsync(currentAdminEmail);
            if (admin == null || admin.Role != "Admin") throw new Exception("Access denied.");

            // Admins cannot see other admins
            return (await _userRepo.GetAllAsync()).Where(u => u.Role != "Admin").ToList();
        }

        public async Task UpdateUserAsync(Guid userId, UpdateUserDto dto, string currentAdminEmail)
        {
            var admin = await _userRepo.GetByEmailAsync(currentAdminEmail);
            var user = await _userRepo.GetByIdAsync(userId);
            if (admin == null || user == null) throw new Exception("Invalid operation.");
            if (user.Role == "Admin") throw new Exception("Admins cannot update other admins.");

            user.Username = dto.Username;
            user.Email = dto.Email;
            await _userRepo.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid userId, string currentAdminEmail)
        {
            var admin = await _userRepo.GetByEmailAsync(currentAdminEmail);
            var user = await _userRepo.GetByIdAsync(userId);
            if (admin == null || user == null) throw new Exception("Invalid operation.");
            if (user.Role == "Admin") throw new Exception("Admins cannot delete other admins.");

            await _userRepo.DeleteAsync(user);
        }
    }
}
