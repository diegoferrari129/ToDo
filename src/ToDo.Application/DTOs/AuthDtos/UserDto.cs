using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Application.DTOs.AuthDtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
