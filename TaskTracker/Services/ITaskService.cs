using Microsoft.AspNetCore.Mvc;
using TaskTracker.Models;

namespace TaskTracker.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetTasks();
        
        Task<TaskItem> GetTaskById(int id);

        Task<TaskItem> CreateTask(TaskItem task);

        Task<bool> DeleteTask(int id);

        Task<bool> UpdateTask(int id, TaskItem updateTask);
    }
}
