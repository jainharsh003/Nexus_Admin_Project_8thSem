using UserDetails.Models;

namespace UserDetails.Repositories.Interface
{
    public interface IUserDetailsRepository
    {
        Task<UserDetailsEntity> CreateAsync(UserDetailsEntity userDetails);
        Task<UserDetailsEntity?> GetByUserIdAsync(Guid userId);
        Task<UserDetailsEntity?> GetByIdAsync(Guid id);
    }
}
