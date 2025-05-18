using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;

namespace TaskTracker.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskItem?> GetTaskById(int id)
        {
            return await _context.Tasks.FindAsync(id); 
        }

        public async Task<List<TaskItem>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<bool> UpdateTask(int id, TaskItem updateTask)
        {
            if (id != updateTask.Id) return false;

            _context.Entry(updateTask).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
