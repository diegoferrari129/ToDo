
namespace ToDo.Application.DTOs.TaskItemDtos
{
    public class CreateTaskItemRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
