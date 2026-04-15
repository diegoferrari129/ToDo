
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
    }
}