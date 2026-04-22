
using System.ComponentModel.DataAnnotations;

namespace ToDo.Application.DTOs.TaskItemDtos
{
    public class CreateTaskItemRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
