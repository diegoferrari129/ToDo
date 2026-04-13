using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces
{
    public interface ITaskItemRepository
    {
        // Read
        Task<TaskItem?> GetByIdAsync(int id, int userId);
        Task<List<TaskItem>> GetUserTasksAsync(int userId);

        // Create
        Task<TaskItem> CreateAsync(TaskItem taskItem);
    }
}
