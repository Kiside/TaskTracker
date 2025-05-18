using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;
using TaskTracker.Services;
using TaskTracker.DTOs;
using Mapster;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var tasks = await _taskService.GetTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemResponse>> GetTask(int id)
        {
            var task = await _taskService.GetTaskById(id);

            if (task != null)
            {
                var response = task.Adapt<TaskItemResponse>();
                return Ok(response);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask(CreateTaskItemRequest task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTask = await _taskService.CreateTask(task);

            return CreatedAtAction(nameof(GetTask), new {id = createdTask.Id}, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskItem task)
        {
            if (id != task.Id)
                return BadRequest();

            var success = await _taskService.UpdateTask(id, task);

            return success ? NoContent() : NotFound();

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var success = await _taskService.DeleteTask(id);

            return success ? NoContent() : NotFound();
        }
    }
}
