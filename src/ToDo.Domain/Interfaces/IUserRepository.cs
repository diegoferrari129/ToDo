using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces
{
    public interface IUserRepository
    {
        // Create
        Task<User> CreateAsync(User user);

        // Read
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByIdWithTasksAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);

        // Update
        Task UpdateUserAsync(User user);
    }
}