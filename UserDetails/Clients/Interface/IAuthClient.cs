using UserDetails.DTOs;

namespace UserDetails.Clients.Interface
{
    public interface IAuthClient
    {
        Task<AuthUserDto?> ValidateTokenAsync(string token);

        Task<AuthUserDto?> GetUserByIdAsync(Guid userId, string token); // optional (keep if needed)
    }
}