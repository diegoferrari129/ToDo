
namespace ToDo.Application.DTOs
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
