
namespace ToDo.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public bool IsCompleted { get; private set; } = false;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? DueDate { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        // FK to User
        public int UserId { get; private set; }
        public User? User { get; private set; }


        public TaskItem(string title, string? description, DateTime? dueDate, int userId)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            UserId = userId;
        }

        protected TaskItem() { }
    }
}
