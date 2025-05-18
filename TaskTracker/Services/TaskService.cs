using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.DTOs;
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

        public async Task<TaskItem> CreateTask(CreateTaskItemRequest taskRequest)
        {
            var task = taskRequest.Adapt<TaskItem>();

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null) return false;

            var result = await _context.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync();
            return result > 0;
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

            _context.Tasks.Update(updateTask);

            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
    }
}
