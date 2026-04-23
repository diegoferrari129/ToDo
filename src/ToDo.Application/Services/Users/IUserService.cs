using ToDo.Application.DTOs.UserDtos;

namespace ToDo.Application.Services.Users
{
    public interface IUserService
    {
        // read operations
        Task<UserProfileResponse> GetByIdAsync(int userId);

        // write operations
        Task<UserProfileResponse> PatchAsync(int userId, UpdateUserRequest request);
        Task<bool> UpdatePasswordAsync(int userId, ChangePasswordRequest request);
        Task<bool> HardDeleteAsync(int userId);
    }
}
