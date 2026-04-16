
namespace ToDo.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        // 1:* relationship with TaskItem
        private readonly List<TaskItem> _taskItems = new();
        public IReadOnlyCollection<TaskItem> TaskItems => _taskItems.AsReadOnly();



        public User(string email, string username, string passwordHash)
        {
            Email = email;
            Username = username;
            PasswordHash = passwordHash;
        }

        // Parameterless constructor for EF Core
        protected User() { }



        // Method to add a new TaskItem to the user's list of tasks
        public TaskItem AddTaskItem(string title, string? description, DateTime? dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");

            var taskItem = new TaskItem(title, description, dueDate, Id);

            _taskItems.Add(taskItem);

            return taskItem;
        }

        // Method to update an existing TaskItem
        public void UpdateTaskItem(int taskId, string title, string? description, bool isCompleted, DateTime? dueDate)
        {
            var taskItem = _taskItems.FirstOrDefault(t => t.Id == taskId);

            if (taskItem == null)
                throw new InvalidOperationException("Task not found");

            taskItem.UpdateTaskItem(title, description, isCompleted, dueDate);
        }

        // Separate methods to update specific properties of a TaskItem
        public void UpdateTaskTitle(int taskId, string newTitle)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");
            task.UpdateTitle(newTitle);
        }

        public void UpdateTaskDescription(int taskId, string? newDescription)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");
            task.UpdateDescription(newDescription);
        }

        public void UpdateTaskDueDate(int taskId, DateTime? newDueDate)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");
            task.UpdateDueDate(newDueDate);
        }

        public void CompleteTask(int taskId)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");
            task.Complete();
        }

        public void ReopenTask(int taskId)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");
            task.Reopen();
        }

        public bool DeleteTask(int taskId)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");
            task.SoftDelete();
            return true;
        }

        public bool RestoreTask(int taskId)
        {
            var task = _taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");
            task.Restore();
            return true;
        }

        public IReadOnlyCollection<TaskItem> ActiveTasks => _taskItems.Where(t => !t.IsDeleted).ToList().AsReadOnly();

        public IReadOnlyCollection<TaskItem> DeletedTasks => _taskItems.Where(t => t.IsDeleted).ToList().AsReadOnly();
    }
}