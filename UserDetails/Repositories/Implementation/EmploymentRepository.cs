using Microsoft.EntityFrameworkCore;
using UserDetails.Data;
using UserDetails.Models;
using UserDetails.Models;
using UserDetails.Repositories.Interface;

namespace Service2.Repositories
{
    public class EmploymentRepository : IEmploymentRepository
    {
        private readonly UserDetailsDbContext _context;

        public EmploymentRepository(UserDetailsDbContext context)
        {
            _context = context;
        }

        public async Task<EmploymentDetails> CreateAsync(EmploymentDetails entity)
        {
            _context.EmploymentDetails.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<EmploymentDetails?> GetByUserIdAsync(Guid userId)
        {
            return await _context.EmploymentDetails
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task UpdateAsync(EmploymentDetails entity)
        {
            _context.EmploymentDetails.Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}