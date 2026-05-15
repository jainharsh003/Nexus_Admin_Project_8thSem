using UserDetails.Clients.Interface;
using UserDetails.DTOs;
using UserDetails.Models;
using UserDetails.Repositories.Interface;
using UserDetails.Services.Interface;

namespace UserDetails.Services
{
    public class UserDetailsService : IUserDetailsService
    {
        private readonly IUserDetailsRepository _repo;
        private readonly IEmploymentRepository _employmentRepo;
        private readonly IAuthClient _authClient;

        public UserDetailsService(
            IUserDetailsRepository repo,
            IEmploymentRepository employmentRepo,
            IAuthClient authClient)
        {
            _repo = repo;
            _employmentRepo = employmentRepo;
            _authClient = authClient;
        }

        // CREATE USER DETAILS
        public async Task<UserDetailsResponseDto> CreateAsync(CreateUserDetailsDto dto, Guid userId)
        {
            var entity = new UserDetailsEntity
            {
                UserId = userId,   // comes from middleware token
                Name = dto.Name,
                FatherName = dto.FatherName,
                MotherName = dto.MotherName,
                DOB = dto.DOB,
                Age = dto.Age,
                Gender = dto.Gender,
                Field = dto.Field
            };

            var created = await _repo.CreateAsync(entity);

            return new UserDetailsResponseDto
            {
                Id = created.Id,
                UserId = created.UserId,
                Name = created.Name,
                FatherName = created.FatherName,
                MotherName = created.MotherName,
                DOB = created.DOB,
                Age = created.Age,
                Gender = created.Gender,
                Field = created.Field
            };
        }

        // GET BY PRIMARY ID
        public async Task<UserDetailsResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("UserDetails not found");

            return new UserDetailsResponseDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name,
                FatherName = entity.FatherName,
                MotherName = entity.MotherName,
                DOB = entity.DOB,
                Age = entity.Age,
                Gender = entity.Gender,
                Field = entity.Field
            };
        }

        // GET FULL DETAILS USING USERID
        public async Task<CombinedUserResponseDto> GetFullDetails(Guid userId, AuthUserDto authUser)
        {
            var entity = await _repo.GetByUserIdAsync(userId);

            if (entity == null)
                throw new Exception("UserDetails not found");

            return new CombinedUserResponseDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Username=authUser.Username,

                // data from Auth Service (via middleware)
                Email = authUser.Email,
                Role = authUser.Role,

                // data from UserDetails DB
                Name = entity.Name,
                FatherName = entity.FatherName,
                MotherName = entity.MotherName,
                DOB = entity.DOB,
                Age = entity.Age,
                Gender = entity.Gender,
                Field = entity.Field
            };
        }
        public async Task<CombinedFullUserDto> GetFullUserProfile(Guid userId, string token)
        {
            var userDetails = await _repo.GetByUserIdAsync(userId);

            if (userDetails == null)
                throw new Exception("User details not found");

            var employment = await _employmentRepo.GetByUserIdAsync(userId);

            var authUser = await _authClient.GetUserByIdAsync(userId, token);

            if (authUser == null)
                throw new Exception("User not found in auth service");

            return new CombinedFullUserDto
            {
                UserId = userId,

                // Service1
                Email = authUser.Email,
                Username = authUser.Username,
                Role = authUser.Role,
                EmploymentID = authUser.EmploymentID,
                PanCard = authUser.PanCard,

                // Service2
                Name = userDetails.Name,
                FatherName = userDetails.FatherName,
                MotherName = userDetails.MotherName,
                DOB = userDetails.DOB,
                Age = userDetails.Age,
                Gender = userDetails.Gender,
                Field = userDetails.Field,

                DOJ = employment?.DOJ ?? DateTime.MinValue,
                AadharCard = employment?.AadharCard
            };
        }
    }
}