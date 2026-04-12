
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

        protected User() { }
    }
}