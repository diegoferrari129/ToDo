using ToDo.Application.DTOs.UserDtos;
using ToDo.Domain.Interfaces;

namespace ToDo.Application.Services.Users
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

        public async Task<UserProfileResponse> GetByIdAsync(int userId)
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

        public async Task<UserProfileResponse> PatchAsync(int userId, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            // ensure email is unique before being updated
            if (!string.IsNullOrEmpty(request.Email))
            {
                var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
                if (existingEmail != null && existingEmail.Id != userId)
                    throw new ArgumentException("Email is already in use");
                user.UpdateEmail(request.Email);
            }

            // ensure username is unique before being updated
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

        public async Task<bool> UpdatePasswordAsync(int userId, ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            // verify current password is correct before allowing password change
            if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Password is incorrect");

            // ensure new password is different from current password
            if (_passwordService.VerifyPassword(request.NewPassword, user.PasswordHash))
                throw new ArgumentException("New password must be different from the current password");

            var newHashedPassword = _passwordService.HashPassword(request.NewPassword);

            user.UpdatePassword(newHashedPassword);

            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> HardDeleteAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            // hard delete the user and all associated tasks with cascade delete to ensure data integrity and avoid orphaned records
            await _userRepository.HardDeleteAsync(user);

            return true;
        }
    }
}