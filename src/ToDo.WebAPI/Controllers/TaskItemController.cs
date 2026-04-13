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
            var task = await _taskItemService.CreateTaskItemAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new {id = task.Id, userId }, task);
        }

        private int GetCurrentUserId()
        {
            return 1;
        }
    }
}
