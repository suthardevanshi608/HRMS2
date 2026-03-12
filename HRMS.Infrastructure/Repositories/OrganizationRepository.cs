using HRMS.Domain.Entities;
using HRMS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class OrganizationRepository
    {
        private readonly ApplicationDbContext _context;

        public OrganizationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
        }

        public async Task<Organization?> GetByIdAsync(string id)
        {
            return await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Organization>> GetAllAsync()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
        }
    }
}
