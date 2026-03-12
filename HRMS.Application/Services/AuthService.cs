using HRMS.Application.DTOs.Auth;
using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterAsync(RegisterRequest dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception("Registration failed");
        }

        public async Task<TokenResponseDto> LoginAsync(LoginRequest dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid credentials");

            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!valid)
                throw new Exception("Invalid credentials");

            return new TokenResponseDto
            {
                Token = "JWT_TOKEN_GENERATION_LOGIC"
            };
        }
    }
}
