using ToDo.Application.DTOs.TaskItemDtos;
using ToDo.Domain.Interfaces;

namespace ToDo.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskItemRepository _taskItemRepository;

        public TaskItemService(IUserRepository userRepository, ITaskItemRepository taskItemRepository)
        {
            _userRepository = userRepository;
            _taskItemRepository = taskItemRepository;
        }

        public async Task<TaskItemResponse> CreateTaskItemAsync(int userId, CreateTaskItemRequest request)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);

            if (user == null)
                throw new Exception("User not found.");

            var taskItem = user.AddTask(request.Title, request.Description, request.DueDate);

            await _userRepository.UpdateAsync(user);

            return new TaskItemResponse
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsCompleted = taskItem.IsCompleted,
                CreatedAt = taskItem.CreatedAt,
                DueDate = taskItem.DueDate,
                CompletedAt = taskItem.CompletedAt
            };
        }

        public async Task<TaskItemResponse> GetTaskItemByIdAsync(int userId, int taskId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId, userId);

            if (task == null)
                throw new Exception("Task not found.");

            return new TaskItemResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                CompletedAt = task.CompletedAt
            };
        }

        public async Task<List<TaskItemResponse>> GetAllTaskItemsAsync(int userId)
        {
            var tasks = await _taskItemRepository.GetUserTasksAsync(userId);

            return tasks.Select(t => new TaskItemResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt
            }).ToList();
        }


    }
}