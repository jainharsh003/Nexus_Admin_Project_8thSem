using UserDetails.DTOs;

namespace UserDetails.Services.Interface
{
    public interface IEmploymentService
    {
        Task CreateEmployment(CreateEmploymentDto dto, Guid userId,string token);

        Task UpdateEmployment(Guid userId, UpdateEmploymentDto dto,string token);
    }
}
