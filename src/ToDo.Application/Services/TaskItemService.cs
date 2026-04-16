using ToDo.Application.DTOs.TaskItemDtos;
using ToDo.Domain.Entities;
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

        #region CRUD Operations

        // Create
        public async Task<TaskItemResponse> CreateTaskItemAsync(int userId, CreateTaskItemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required", nameof(request.Title));

            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            var taskItem = user.CreateTaskItem(request.Title, request.Description, request.DueDate);

            await _userRepository.UpdateAsync(user);

            return MapToResponse(taskItem);
        }

        // Get by ID
        public async Task<TaskItemResponse> GetTaskItemByIdAsync(int userId, int taskId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId, userId);
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            return MapToResponse(task);
        }

        // Get all
        public async Task<List<TaskItemResponse>> GetAllTaskItemsAsync(int userId)
        {
            var tasks = await _taskItemRepository.GetUserTasksAsync(userId);

            return tasks.Select(MapToResponse).ToList();
        }

        // Update
        public async Task<TaskItemResponse?> UpdateTaskItemAsync(int userId, int taskId, UpdateTaskItemRequest request)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            var taskItem = user.TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (taskItem == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            user.UpdateTaskItem(taskId, request.Title, request.Description, request.IsCompleted, request.DueDate);

            await _userRepository.UpdateAsync(user);

            var updatedTask = user.TaskItems.First(t => t.Id == taskId);

            return MapToResponse(updatedTask);
        }

        // Patch
        public async Task<TaskItemResponse?> PatchTaskItemAsync(int userId, int taskId, PatchTaskItemRequest request)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            var taskItem = user.TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (taskItem == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

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

        // Soft delete
        public async Task<bool> SoftDeleteTaskItemAsync(int userId, int taskId)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            var success = user.DeleteTask(taskId);
            if (!success)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            await _userRepository.UpdateAsync(user);

            return true;
        }

        // Restore TaskItem
        public async Task<bool> RestoreTaskItemAsync(int userId, int taskId)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            var success = user.RestoreTask(taskId);
            if (!success)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            await _userRepository.UpdateAsync(user);

            return true;
        }

        // Get deleted TaskItems list
        public async Task<List<TaskItemResponse>> GetDeletedTaskItemsAsync(int userId)
        {
            var tasks = await _taskItemRepository.GetUserDeletedTasksAsync(userId);

            return tasks.Select(MapToResponse).ToList();
        }

        #endregion

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