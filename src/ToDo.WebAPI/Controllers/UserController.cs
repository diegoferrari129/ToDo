using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDo.Application.DTOs.UserDtos;
using ToDo.Application.Services.Users;

namespace ToDo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();

            var user = await _userService.GetByIdAsync(userId);

            return Ok(user);
        }

        [HttpPatch("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserRequest request)
        {
            var userId = GetCurrentUserId();

            var updatedUser = await _userService.PatchAsync(userId, request);

            return Ok(updatedUser);
        }

        [HttpPatch("me/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = GetCurrentUserId();

            await _userService.UpdatePasswordAsync(userId, request);

            return Ok(new { message = "Password changed successfully" });
        }

        [HttpDelete("me/delete")]
        public async Task<IActionResult> HardDeleteAccount([FromQuery] bool confirm = false)
        {
            var userId = GetCurrentUserId();

            await _userService.HardDeleteAsync(userId);

            return Ok(new { message = "Account eliminato definitivamente" });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.Parse(userIdClaim!);
        }
    }
}
