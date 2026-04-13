using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByIdWithTasksAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
    }
}