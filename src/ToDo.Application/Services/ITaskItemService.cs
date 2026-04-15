using ToDo.Application.DTOs.TaskItemDtos;

namespace ToDo.Application.Services
{
    public interface ITaskItemService
    {
        // TaskItem CRUD operations
        Task<TaskItemResponse> CreateAsync(int userId, CreateTaskItemRequest request);
        Task<TaskItemResponse> GetTaskItemByIdAsync(int userId, int taskId);
        Task<List<TaskItemResponse>> GetAllTaskItemsAsync(int userId);
        Task<TaskItemResponse?> UpdateAsync(int taskId, int userId, UpdateTaskItemRequest request);
    }
}
