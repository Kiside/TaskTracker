using Microsoft.AspNetCore.Mvc;
using TaskTracker.DTOs;
using TaskTracker.Models;

namespace TaskTracker.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetTasks();
        
        Task<TaskItem?> GetTaskById(int id);

        Task<TaskItem> CreateTask(CreateTaskItemRequest task);

        Task<bool> DeleteTask(int id);

        Task<bool> UpdateTask(int id, TaskItem updateTask);
    }
}
