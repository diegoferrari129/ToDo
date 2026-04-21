namespace ToDo.Application.DTOs.UserDtos
{
    public class UserProfileResponse
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
