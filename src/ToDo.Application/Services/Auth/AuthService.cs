using ToDo.Application.DTOs.AuthDtos;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces;

namespace ToDo.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> RegisterAsync(UserRegisterRequest request)
        {
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingEmail != null)
                return new AuthResponse { Success = false, Message = "Email already registered" };

            var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUsername != null)
                return new AuthResponse { Success = false, Message = "Username already in use" };

            var user = new User(
                request.Email,
                request.Username,
                _passwordService.HashPassword(request.Password)
            );

            await _userRepository.CreateAsync(user);

            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                User = new UserResponse { Id = user.Id, Email = user.Email, Username = user.Username }
            };
        }

        public async Task<AuthResponse> LoginAsync(UserLoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return new AuthResponse { Success = false, Message = "Invalid email" };

            if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                return new AuthResponse { Success = false, Message = "Invalid password" };

            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                User = new UserResponse { Id = user.Id, Email = user.Email, Username = user.Username }
            };
        }
    }
}