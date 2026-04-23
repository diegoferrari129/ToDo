using ToDo.Application.DTOs.TaskItemDtos;

namespace ToDo.Application.Services.TaskItems
{
    public interface IUserTaskService
    {
        Task<TaskItemResponse> CreateAsync(int userId, CreateTaskItemRequest request);
        Task<TaskItemResponse?> UpdateAsync(int userId, int taskId, UpdateTaskItemRequest request);
        Task<TaskItemResponse?> PatchAsync(int userId, int taskId, PatchTaskItemRequest request);
        Task<bool> SoftDeleteAsync(int userId, int taskId);
        Task<bool> RestoreAsync(int userId, int taskId);
    }
}
