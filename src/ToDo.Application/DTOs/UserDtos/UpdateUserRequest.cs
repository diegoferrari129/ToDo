using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToDo.Application.DTOs.UserDtos
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [MaxLength(25, ErrorMessage = "Username cannot exceed 25 characters")]
        public string Username { get; set; } = string.Empty;
    }
}
