namespace ToDo.Domain.Entities
{
    // *:1 relationship: User
    public class TaskItem
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public bool IsCompleted { get; private set; } = false;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? DueDate { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }

        // FK
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

        public void Update(string title, string? description, bool isCompleted, DateTime? dueDate)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;

            if (isCompleted && !IsCompleted)
            {
                IsCompleted = true;
                CompletedAt = DateTime.UtcNow;
            }
            else if (!isCompleted && IsCompleted)
            {
                IsCompleted = false;
                CompletedAt = null;
            }
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentNullException(nameof(newTitle), "Il titolo è obbligatorio");

            Title = newTitle;
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription;
        }

        public void UpdateDueDate(DateTime? newDueDate)
        {
            DueDate = newDueDate;
        }

        public void Complete()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                CompletedAt = DateTime.UtcNow;
            }
        }

        public void Reopen()
        {
            if (IsCompleted)
            {
                IsCompleted = false;
                CompletedAt = null;
            }
        }

        public void SoftDelete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                DeletedAt = DateTime.UtcNow;
            }
        }

        public void Restore()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                DeletedAt = null;
            }
        }
    }
}