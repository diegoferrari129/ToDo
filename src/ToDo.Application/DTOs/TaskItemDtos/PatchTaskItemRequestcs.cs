using System.ComponentModel.DataAnnotations;

namespace ToDo.Application.DTOs.TaskItemDtos
{
    public class PatchTaskItemRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
