using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces;
using ToDo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Infrastructure.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;

        public TaskItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(int id, int userId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<List<TaskItem>> GetUserTasksAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetUserDeletedTasksAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId && t.IsDeleted)
                .OrderByDescending(t => t.DeletedAt)
                .ToListAsync();
        }

        public async Task<TaskItem> CreateAsync(TaskItem taskItem)
        {
            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }
    }
}
