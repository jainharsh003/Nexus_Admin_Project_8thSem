using LoginSignup.DTOs;
using LoginSignup.Models;
using LoginSignup.Repositories.Interfaces;
using LoginSignup.Services.Interfaces;

namespace LoginSignup.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<User>> GetAllUsersAsync(string currentAdminEmail);
        Task UpdateUserAsync(Guid userId, UpdateUserDto dto, string currentAdminEmail);
        Task DeleteUserAsync(Guid userId, string currentAdminEmail);
    }
}