using ToDo.Application.DTOs.AuthDtos;

namespace ToDo.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(UserRegisterRequest request);
        Task<AuthResponse> LoginAsync(UserLoginRequest request);
    }
}
