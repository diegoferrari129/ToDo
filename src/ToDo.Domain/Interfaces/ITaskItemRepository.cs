using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces
{
    public interface ITaskItemRepository
    {
        // Create
        Task<TaskItem> CreateAsync(TaskItem taskItem);

        // Read
        Task<TaskItem?> GetByIdAsync(int id, int userId);
        Task<List<TaskItem>> GetUserTasksAsync(int userId);
        Task<List<TaskItem>> GetUserDeletedTasksAsync(int userId);

    }
}
