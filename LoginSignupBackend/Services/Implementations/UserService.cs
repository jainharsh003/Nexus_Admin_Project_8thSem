using LoginSignup.DTOs;
using LoginSignup.Models;
using LoginSignup.Repositories.Interfaces;
using LoginSignup.Services.Interfaces;

namespace LoginSignup.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepo) => _userRepo = userRepo;

        public async Task<User> GetProfileAsync(string userEmail)
        {
            var user = await _userRepo.GetByEmailAsync(userEmail);
            if (user == null) throw new Exception("User not found.");
            return user;
        }

        public async Task UpdateProfileAsync(UpdateUserDto dto, string userEmail)
        {
            var user = await _userRepo.GetByEmailAsync(userEmail);
            if (user == null) throw new Exception("User not found.");

            user.Username = dto.Username;
            user.Email = dto.Email;

            await _userRepo.UpdateAsync(user);
        }
        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");

            return user;
        }
        public async Task UpdateEmploymentAsync(Guid userId, string employmentId, string panCard)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.EmploymentID = employmentId;
            user.PanCard = panCard;

            await _userRepo.UpdateAsync(user);
        }
    }
}
