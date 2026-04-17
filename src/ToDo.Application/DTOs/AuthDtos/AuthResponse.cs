using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Application.DTOs.AuthDtos
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }      
    }
}
