using HRMS.Domain.Entities;
using HRMS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class HRRepository
    {
        private readonly ApplicationDbContext _context;

        public HRRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetAllHRsAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
