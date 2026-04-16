using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces
{
    public interface ITaskItemRepository
    {
        Task<TaskItem?> GetByIdAsync(int id, int userId);
        Task<List<TaskItem>> GetUserTasksAsync(int userId);
        Task<List<TaskItem>> GetUserDeletedTasksAsync(int userId);
    }
}
