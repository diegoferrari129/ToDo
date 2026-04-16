using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTOs.TaskItemDtos;
using ToDo.Application.Services;

namespace ToDo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> GetById(int id, int userId)
        {
            var task = await _taskItemService.GetTaskItemByIdAsync(id, userId);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemRequest request)
        {
            var userId = GetCurrentUserId();
            var task = await _taskItemService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = task.Id, userId }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemRequest request)
        {
            var userId = GetCurrentUserId();

            var updatedTaskItem = await _taskItemService.UpdateAsync(userId, id, request);

            if(updatedTaskItem == null)
                return NotFound("Task not found");

            return Ok(updatedTaskItem);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchTaskItemRequest request)
        {
            var userId = GetCurrentUserId();

            var updated = await _taskItemService.PatchTaskItemAsync(userId, id, request);

            if (updated == null)
                return NotFound("Task not found");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var userId = GetCurrentUserId();

            var success = await _taskItemService.SoftDeleteTaskItemAsync(userId, id);

            if (!success)
                return NotFound(new { message = "Task non trovato" });

            return Ok(new { message = "Task spostato nel cestino" });
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var userId = GetCurrentUserId();

            var success = await _taskItemService.RestoreTaskItemAsync(userId, id);

            if (!success)
                return NotFound(new { message = "Task non trovato" });

            return Ok(new { message = "Task ripristinato" });
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
            return 1;
        }
    }
}
