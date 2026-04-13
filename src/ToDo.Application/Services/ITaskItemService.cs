using ToDo.Application.DTOs.TaskItemDtos;

namespace ToDo.Application.Services
{
    public interface ITaskItemService
    {
        Task<TaskItemResponse> CreateTaskItemAsync(int userId, CreateTaskItemRequest request);
        Task<TaskItemResponse> GetTaskItemByIdAsync(int userId, int taskId);
        Task<List<TaskItemResponse>> GetAllTaskItemsAsync(int userId);
    }
}
