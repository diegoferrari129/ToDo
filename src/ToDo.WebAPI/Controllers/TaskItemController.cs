using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDo.Application.DTOs.TaskItemDtos;
using ToDo.Application.Services.TaskItems;

namespace ToDo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _readService;
        private readonly IUserTaskService _writeService;

        public TaskItemController(ITaskItemService readService, IUserTaskService writeService)
        {
            _readService = readService;
            _writeService = writeService;
        }

        [HttpGet("/read")]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();

            var tasks = await _readService.GetAllAsync(userId);

            return Ok(tasks);
        }

        [HttpGet("/read/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetCurrentUserId();

            var task = await _readService.GetByIdAsync(userId, id);

            return Ok(task);
        }

        [HttpPost("/create")]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemRequest request)
        {
            var userId = GetCurrentUserId();

            var task = await _writeService.CreateAsync(userId, request);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("/update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemRequest request)
        {
            var userId = GetCurrentUserId();

            var updatedTaskItem = await _writeService.UpdateAsync(userId, id, request);

            return Ok(updatedTaskItem);
        }

        [HttpPatch("/patch/{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchTaskItemRequest request)
        {
            var userId = GetCurrentUserId();

            var updated = await _writeService.PatchAsync(userId, id, request);

            return Ok(updated);
        }

        [HttpDelete("/delete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var userId = GetCurrentUserId();

            await _writeService.SoftDeleteAsync(userId, id);

            return Ok(new { message = "Task moved to trash" });
        }

        [HttpPatch("/restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var userId = GetCurrentUserId();

            await _writeService.RestoreAsync(userId, id);

            return Ok(new { message = "Task restored" });
        }

        [HttpGet("/read/deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            var userId = GetCurrentUserId();

            var tasks = await _readService.GetDeletedAsync(userId);

            return Ok(tasks);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not found");

            return int.Parse(userIdClaim);
        }
    }
}