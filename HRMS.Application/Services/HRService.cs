using HRMS.Application.DTOs.HR;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using HRMS.Domain.Entities;

namespace HRMS.Application.Services
{
    public class HRService : IHRService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HRService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task CreateHRAsync(CreateHRDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception("HR creation failed");

            await _userManager.AddToRoleAsync(user, "HR");
        }
    }
}
