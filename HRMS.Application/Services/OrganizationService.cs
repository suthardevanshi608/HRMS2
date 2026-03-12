using HRMS.Application.DTOs.Organization;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Repositories;

namespace HRMS.Application.Services
{
    public class OrganizationService
    {
        private readonly OrganizationRepository _repository;

        public OrganizationService(OrganizationRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(CreateOrganizationDto dto, string userId)
        {
            var organization = new Organization
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Address = dto.Address,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(organization);
        }
    }
}
