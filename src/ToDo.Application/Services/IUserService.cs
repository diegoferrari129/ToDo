using ToDo.Application.DTOs.UserDtos;

namespace ToDo.Domain.Interfaces
{
    public interface IUserService
    {
        // Read operations
        Task<UserProfileResponse> GetUserProfileAsync(int userId);

        // Write operations
        Task<UserProfileResponse> PatchUserProfileAsync(int userId, UpdateUserRequest request);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<bool> DeleteUserAsync(int userId);
    }
}
