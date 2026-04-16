using ToDo.Application.DTOs.TaskItemDtos;

namespace ToDo.Application.Services
{
    public interface ITaskItemService
    {
        // CRUD operations
        Task<TaskItemResponse> CreateTaskItemAsync(int userId, CreateTaskItemRequest request);
        Task<TaskItemResponse> GetTaskItemByIdAsync(int userId, int taskId);
        Task<List<TaskItemResponse>> GetAllTaskItemsAsync(int userId);
        Task<TaskItemResponse?> UpdateTaskItemAsync(int userId, int taskId, UpdateTaskItemRequest request);
        Task<TaskItemResponse?> PatchTaskItemAsync(int userId, int taskId, PatchTaskItemRequest request);
        Task<bool> SoftDeleteTaskItemAsync(int userId, int taskId);
        Task<List<TaskItemResponse>> GetDeletedTaskItemsAsync(int userId);
        Task<bool> RestoreTaskItemAsync(int userId, int taskId);
    }
}
