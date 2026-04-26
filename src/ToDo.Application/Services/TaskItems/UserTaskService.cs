using ToDo.Application.DTOs.TaskItemDtos;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces;

namespace ToDo.Application.Services.TaskItems
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskItemRepository _taskItemRepository;

        public UserTaskService(IUserRepository userRepository, ITaskItemRepository taskItemRepository)
        {
            _userRepository = userRepository;
            _taskItemRepository = taskItemRepository;
        }

        public async Task<TaskItemResponse> CreateAsync(int userId, CreateTaskItemRequest request)
        {
            if (request.DueDate.HasValue && request.DueDate.Value.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Due date cannot be in the past");

            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User not found");

            var taskItem = user.CreateTaskItem(request.Title, request.Description, request.DueDate);

            await _userRepository.UpdateAsync(user);

            return MapToResponse(taskItem);
        }

        public async Task<TaskItemResponse?> UpdateAsync(int userId, int taskId, UpdateTaskItemRequest request)
        {
            if (request.DueDate.HasValue && request.DueDate.Value.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Due date cannot be in the past");

            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var taskItem = user.TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (taskItem == null)
                throw new KeyNotFoundException("Task not found");

            user.UpdateTaskItem(taskId, request.Title, request.Description, request.IsCompleted, request.DueDate);

            await _userRepository.UpdateAsync(user);

            var updatedTask = user.TaskItems.First(t => t.Id == taskId);

            return MapToResponse(updatedTask);
        }

        public async Task<TaskItemResponse?> PatchAsync(int userId, int taskId, PatchTaskItemRequest request)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var taskItem = user.TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (taskItem == null)
                throw new KeyNotFoundException("Task not found");

            if (request.DueDate.HasValue && request.DueDate.Value.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Due date cannot be in the past");

            if (request.Title != null)
                user.UpdateTaskTitle(taskId, request.Title);

            if (request.Description != null)
                user.UpdateTaskDescription(taskId, request.Description);

            if (request.IsCompleted.HasValue)
            {
                if (request.IsCompleted.Value)
                    user.CompleteTask(taskId);
                else
                    user.ReopenTask(taskId);
            }

            if (request.DueDate.HasValue)
                user.UpdateTaskDueDate(taskId, request.DueDate.Value);

            await _userRepository.UpdateAsync(user);

            var updatedTask = user.TaskItems.First(t => t.Id == taskId);

            return MapToResponse(updatedTask);
        }

        public async Task<bool> SoftDeleteAsync(int userId, int taskId)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var success = user.SoftDeleteTaskItem(taskId);
            if (!success)
                throw new KeyNotFoundException("Task not found");
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> RestoreAsync(int userId, int taskId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId, userId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            if (!task.IsDeleted)
                return false;

            task.Restore();
            await _taskItemRepository.RestoreAsync(task);
            return true;
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
