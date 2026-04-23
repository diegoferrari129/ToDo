namespace ToDo.Application.DTOs.AuthDtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
