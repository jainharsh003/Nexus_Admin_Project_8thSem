using UserDetails.DTOs;
using UserDetails.Models;

namespace UserDetails.Repositories.Interface
{
    public interface IEmploymentRepository
    {
        Task<EmploymentDetails> CreateAsync(EmploymentDetails entity);

        Task<EmploymentDetails?> GetByUserIdAsync(Guid userId);

        Task UpdateAsync(EmploymentDetails entity);
    }
}
