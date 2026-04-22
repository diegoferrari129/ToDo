using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDo.Application.DTOs.UserDtos;
using ToDo.Domain.Interfaces;

namespace ToDo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();

            var user = await _userService.GetUserProfileAsync(userId);

            return Ok(new
            {
                user.Email,
                user.Username,
                user.CreatedAt
            });
        }

        [HttpPatch("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserRequest request)
        {
            var userId = GetCurrentUserId();

            var updatedUser = await _userService.PatchUserProfileAsync(userId, request);

            return Ok(updatedUser);
        }

        [HttpPatch("me/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = GetCurrentUserId();

            await _userService.ChangePasswordAsync(userId, request);

            return Ok(new { message = "Password changed successfully" });
        }

        [HttpDelete("me/permanent")]
        public async Task<IActionResult> HardDeleteAccount([FromQuery] bool confirm = false)
        {
            var userId = GetCurrentUserId();

            await _userService.DeleteUserAsync(userId);

            return Ok(new { message = "Account eliminato definitivamente" });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.Parse(userIdClaim!);
        }


    }
}
