using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Application.DTOs.UserDtos
{
    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
