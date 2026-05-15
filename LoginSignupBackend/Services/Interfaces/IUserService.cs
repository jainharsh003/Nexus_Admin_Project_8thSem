using LoginSignup.DTOs;
using LoginSignup.Models;

namespace LoginSignup.Services.Interfaces
{
    public interface IUserService

    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetProfileAsync(string userEmail);
        Task UpdateProfileAsync(UpdateUserDto dto, string userEmail);
        Task UpdateEmploymentAsync(Guid userId, string employmentId, string panCard);
    }
}
