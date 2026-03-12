using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Application.DTOs.Auth;

namespace HRMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(LoginRequest dto);
        Task RegisterAsync(RegisterRequest dto);
    }
}
