using ToDo.Application.DTOs.AuthDtos;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces;

namespace ToDo.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> RegisterAsync(UserRegisterRequest request)
        {
            // validations
            if (string.IsNullOrWhiteSpace(request.Email))
                return new AuthResponse { Success = false, Message = "Email required" };

            if (string.IsNullOrWhiteSpace(request.Username))
                return new AuthResponse { Success = false, Message = "Username required" };

            if (request.Password.Length < 6)
                return new AuthResponse { Success = false, Message = "Password must be at least 6 characters" };

            // email exits
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingEmail != null)
                return new AuthResponse { Success = false, Message = "Email already registered" };

            // username exists
            var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUsername != null)
                return new AuthResponse { Success = false, Message = "Username already in use" };

            // create new user
            var user = new User(
                request.Email,
                request.Username,
                _passwordService.HashPassword(request.Password)
            );

            await _userRepository.CreateAsync(user);

            // token generation
            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                User = new UserDto { Id = user.Id, Email = user.Email, Username = user.Username }
            };
        }

        public async Task<AuthResponse> LoginAsync(UserLoginRequest request)
        {
            // find user by email
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return new AuthResponse { Success = false, Message = "Invalid email" };

            // validate password
            if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                return new AuthResponse { Success = false, Message = "Invalid password" };

            // token generation
            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                User = new UserDto { Id = user.Id, Email = user.Email, Username = user.Username }
            };
        }
    }
}