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

        //Crud operations for TaskItems
        #region
        // Create a new TaskItem for a user
        public async Task<TaskItemResponse> CreateAsync(int userId, CreateTaskItemRequest request)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);

            if (user == null)
                throw new Exception("User not found.");

            var taskItem = user.AddTaskItem(request.Title, request.Description, request.DueDate);

            await _userRepository.UpdateUserAsync(user);

            return new TaskItemResponse
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsCompleted = taskItem.IsCompleted,
                CreatedAt = taskItem.CreatedAt,
                DueDate = taskItem.DueDate,
                CompletedAt = taskItem.CompletedAt,
                IsDeleted = taskItem.IsDeleted,
                DeletedAt = taskItem.DeletedAt
            };
        }

        // Get a specific TaskItem by its ID for a user
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
                CompletedAt = task.CompletedAt,
                IsDeleted = task.IsDeleted,
                DeletedAt = task.DeletedAt
            };
        }

        // Get all TaskItems for a user
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
                CompletedAt = t.CompletedAt,
                IsDeleted = t.IsDeleted,
                DeletedAt = t.DeletedAt
            }).ToList();
        }

        // Update an existing TaskItem for a user
        public async Task<TaskItemResponse?> UpdateAsync(int taskId, int userId, UpdateTaskItemRequest request)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                return null;

            var taskItem = user.TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (taskItem == null)
                return null;

            user.UpdateTaskItem(taskId, request.Title, request.Description, request.IsCompleted, request.DueDate);

            await _userRepository.UpdateUserAsync(user);

            return new TaskItemResponse
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsCompleted = taskItem.IsCompleted,
                CreatedAt = taskItem.CreatedAt,
                DueDate = taskItem.DueDate,
                CompletedAt = taskItem.CompletedAt,
                IsDeleted = taskItem.IsDeleted,
                DeletedAt = taskItem.DeletedAt
            };
        }

        // Patch an existing TaskItem for a user
        public async Task<TaskItemResponse?> PatchTaskItemAsync(int userId, int taskId, PatchTaskItemRequest request)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null)
                return null;

            var taskItem = user.TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (taskItem == null)
                return null;

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

            await _userRepository.UpdateUserAsync(user);

            return new TaskItemResponse
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsCompleted = taskItem.IsCompleted,
                CreatedAt = taskItem.CreatedAt,
                DueDate = taskItem.DueDate,
                IsDeleted = taskItem.IsDeleted,
                DeletedAt = taskItem.DeletedAt,
                CompletedAt = taskItem.CompletedAt
            };
        }

        public async Task<bool> SoftDeleteTaskItemAsync(int userId, int taskId)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null) return false;

            var success = user.DeleteTask(taskId);
            if (!success) return false;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<bool> RestoreTaskItemAsync(int userId, int taskId)
        {
            var user = await _userRepository.GetByIdWithTasksAsync(userId);
            if (user == null) return false;

            var success = user.RestoreTask(taskId);
            if (!success) return false;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<List<TaskItemResponse>> GetDeletedTaskItemsAsync(int userId)
        {
            var tasks = await _taskItemRepository.GetUserDeletedTasksAsync(userId);

            return tasks.Select(t => new TaskItemResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                IsDeleted = t.IsDeleted,
                DeletedAt = t.DeletedAt
            }).ToList();
        }
        #endregion
    }
}