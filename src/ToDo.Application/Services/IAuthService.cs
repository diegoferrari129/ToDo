using System;
using System.Collections.Generic;
using System.Text;
using ToDo.Application.DTOs.AuthDtos;

namespace ToDo.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(UserRegisterRequest request);
        Task<AuthResponse> LoginAsync(UserLoginRequest request);
    }
}
