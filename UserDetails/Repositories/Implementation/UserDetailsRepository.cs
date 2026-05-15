using Microsoft.EntityFrameworkCore;
using UserDetails.Data;
using UserDetails.DTOs;
using UserDetails.Mappings;
using UserDetails.Models;
using UserDetails.Repositories.Interface;

namespace UserDetails.Repositories.Implementation
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly UserDetailsDbContext _context;

        public UserDetailsRepository(UserDetailsDbContext context)
        {
            _context = context;
        }

        public async Task<UserDetailsEntity> CreateAsync(UserDetailsEntity userDetails)
        {
            _context.UserDetails.Add(userDetails);
            await _context.SaveChangesAsync();
            return userDetails;
        }

        public async Task<UserDetailsEntity?> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserDetails
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task<UserDetailsEntity?> GetByIdAsync(Guid id)
        {
            return await _context.UserDetails
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
