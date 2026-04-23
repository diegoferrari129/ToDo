using ToDo.Application.DTOs.TaskItemDtos;

namespace ToDo.Application.Services.TaskItems
{
    public interface ITaskItemService
    {
        // all write operations must be done through the aggregate root (User) in UserTaskService, so we only have read operations here
        Task<TaskItemResponse> GetByIdAsync(int userId, int taskId);
        Task<List<TaskItemResponse>> GetAllAsync(int userId);
        Task<List<TaskItemResponse>> GetDeletedAsync(int userId);
    }
}
