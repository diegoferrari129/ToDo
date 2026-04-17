using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDo.Application.DTOs.TaskItemDtos;
using ToDo.Application.Services;

namespace ToDo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;
        private readonly ILogger<TaskItemController> _logger;

        public TaskItemController(ITaskItemService taskItemService, ILogger<TaskItemController> logger)
        {
            _taskItemService = taskItemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();

            var tasks = await _taskItemService.GetAllTaskItemsAsync(userId);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetCurrentUserId();

            var task = await _taskItemService.GetTaskItemByIdAsync(userId, id);

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemRequest request)
        {
            var userId = GetCurrentUserId();

            var task = await _taskItemService.CreateTaskItemAsync(userId, request);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemRequest request)
        {
            var userId = GetCurrentUserId();

            var updatedTaskItem = await _taskItemService.UpdateTaskItemAsync(userId, id, request);

            return Ok(updatedTaskItem);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchTaskItemRequest request)
        {
            var userId = GetCurrentUserId();

            var updated = await _taskItemService.PatchTaskItemAsync(userId, id, request);

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var userId = GetCurrentUserId();

            await _taskItemService.SoftDeleteTaskItemAsync(userId, id);

            return Ok(new { message = "Task moved to trash" });
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var userId = GetCurrentUserId();

            await _taskItemService.RestoreTaskItemAsync(userId, id);

            return Ok(new { message = "Task restored" });
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            var userId = GetCurrentUserId();

            var tasks = await _taskItemService.GetDeletedTaskItemsAsync(userId);

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