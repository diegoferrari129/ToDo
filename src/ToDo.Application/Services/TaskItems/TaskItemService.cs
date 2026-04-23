using ToDo.Application.DTOs.TaskItemDtos;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces;

namespace ToDo.Application.Services.TaskItems
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public TaskItemService(IUserRepository userRepository, ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        // all write operations must be done through the aggregate root (User) in UserTaskService, so we only have read operations here
        public async Task<TaskItemResponse> GetByIdAsync(int userId, int taskId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId, userId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            return MapToResponse(task);
        }

        public async Task<List<TaskItemResponse>> GetAllAsync(int userId)
        {
            var tasks = await _taskItemRepository.GetAllAsync(userId);

            return tasks.Select(MapToResponse).ToList();
        }

        public async Task<List<TaskItemResponse>> GetDeletedAsync(int userId)
        {
            var tasks = await _taskItemRepository.GetDeletedAsync(userId);

            return tasks.Select(MapToResponse).ToList();
        }

        #region Private Helpers
        private static TaskItemResponse MapToResponse(TaskItem task)
        {
            return new TaskItemResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                CompletedAt = task.CompletedAt,
                IsDeleted = task.IsDeleted,
                DeletedAt = task.DeletedAt
            };
        }
        #endregion
    }
}