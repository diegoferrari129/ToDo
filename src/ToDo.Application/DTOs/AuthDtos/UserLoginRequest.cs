using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Application.DTOs.AuthDtos
{
    public class UserLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
