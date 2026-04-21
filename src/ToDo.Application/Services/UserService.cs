using ToDo.Application.DTOs.UserDtos;
using ToDo.Domain.Interfaces;

namespace ToDo.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        public UserService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<UserProfileResponse> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return new UserProfileResponse
            {
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserProfileResponse> PatchUserProfileAsync(int userId, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrEmpty(request.Email))
            {
                var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
                if (existingEmail != null && existingEmail.Id != userId)
                    throw new ArgumentException("Email is already in use");

                user.UpdateEmail(request.Email);
            }

            if (!string.IsNullOrEmpty(request.Username))
            {
                var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
                if (existingUsername != null && existingUsername.Id != userId)
                    throw new ArgumentException("Username is already in use");

                user.UpdateUsername(request.Username);
            }

            await _userRepository.UpdateAsync(user);

            return new UserProfileResponse
            {
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect");
            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
                throw new ArgumentException("New password must be at least 6 characters long");

            var newHashedPassword = _passwordService.HashPassword(request.NewPassword);

            user.UpdatePassword(newHashedPassword);

            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            await _userRepository.HardDeleteAsync(user);

            return true;
        }
    }
}
