using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces
{
    public interface ITaskItemRepository
    {
        // all write operations must be done through the aggregate root (User)
        Task<TaskItem?> GetByIdAsync(int id, int userId);
        Task<List<TaskItem>> GetAllAsync(int userId);
        Task<List<TaskItem>> GetDeletedAsync(int userId);
    }
}
