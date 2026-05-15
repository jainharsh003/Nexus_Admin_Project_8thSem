using UserDetails.DTOs;

namespace UserDetails.Services.Interface
{
    public interface IUserDetailsService
    {
        Task<UserDetailsResponseDto> CreateAsync(CreateUserDetailsDto dto, Guid userId);

        Task<UserDetailsResponseDto> GetByIdAsync(Guid id);

        // userId comes from middleware, authUser contains email + role
        Task<CombinedUserResponseDto> GetFullDetails(Guid userId, AuthUserDto authUser);
        Task<CombinedFullUserDto> GetFullUserProfile(Guid userId, string token);
    }
}