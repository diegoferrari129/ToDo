namespace ToDo.Domain.Entities
{
    public class User
    {
        // 1:* relationship with TaskItem
        public int Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        private readonly List<TaskItem> _taskItems = new();
        public IReadOnlyCollection<TaskItem> TaskItems => _taskItems.AsReadOnly();
        public IReadOnlyCollection<TaskItem> ActiveTasks => _taskItems.Where(t => !t.IsDeleted).ToList().AsReadOnly();
        public IReadOnlyCollection<TaskItem> DeletedTasks => _taskItems.Where(t => t.IsDeleted).ToList().AsReadOnly();

        public User(string email, string username, string passwordHash)
        {
            Email = email;
            Username = username;
            PasswordHash = passwordHash;
        }

        protected User() { }

        #region User Management
        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email cannot be empty.");

            Email = newEmail;
        }

        public void UpdateUsername(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
                throw new ArgumentException("Username cannot be empty.");

            Username = newUsername;
        }

        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password cannot be empty.");

            PasswordHash = newPasswordHash;
        }
        #endregion

        #region TaskItem Management
        public TaskItem CreateTaskItem(string title, string? description, DateTime? dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");

            var taskItem = new TaskItem(title, description, dueDate, Id);

            _taskItems.Add(taskItem);

            return taskItem;
        }

        public void UpdateTaskItem(int taskId, string title, string? description, bool isCompleted, DateTime? dueDate)
        {
            var taskItem = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (taskItem == null)
                throw new KeyNotFoundException("Task not found");

            taskItem.Update(title, description, isCompleted, dueDate);
        }

        public void UpdateTaskTitle(int taskId, string newTitle)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            task.UpdateTitle(newTitle);
        }

        public void UpdateTaskDescription(int taskId, string? newDescription)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            task.UpdateDescription(newDescription);
        }

        public void UpdateTaskDueDate(int taskId, DateTime? newDueDate)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            task.UpdateDueDate(newDueDate);
        }

        public void CompleteTask(int taskId)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            task.Complete();
        }

        public void ReopenTask(int taskId)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            task.Reopen();
        }

        // soft delete
        public bool SoftDeleteTaskItem(int taskId)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            task.SoftDelete();

            return true;
        }

        public bool RestoreTaskItem(int taskId)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            if (!task.IsDeleted) return false;

            task.Restore();
            return true;
        }
        #endregion
    }
}