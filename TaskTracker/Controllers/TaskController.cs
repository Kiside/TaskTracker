using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;
using TaskTracker.Services;
using TaskTracker.DTOs;
using Mapster;
using TaskTracker.Common;
using TaskTracker.Exstensions;

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

        [ProducesResponseType(typeof(ApiResponse<List<TaskItem>>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var tasks = await _taskService.GetTasks();
            return Ok(ApiResponse<List<TaskItem>>.Success(tasks, "Tasks returned"));
        }

        [ProducesResponseType(typeof(ApiResponse<TaskItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TaskItemResponse>), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemResponse>> GetTask(int id)
        {
            var task = await _taskService.GetTaskById(id);

            if (task != null)
            {
                var response = task.Adapt<TaskItemResponse>();
                return Ok(ApiResponse<TaskItemResponse>.Success(response, "Task returned"));
            }

            return NotFound(ApiResponse<TaskItemResponse>.NotFound("Task not found"));
        }

        [ProducesResponseType(typeof(ApiResponse<CreateTaskItemRequest>), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask(CreateTaskItemRequest task)
        {
            if (!ModelState.IsValid)
                return this.ValidationError(ModelState, "Bad Request");

            var createdTask = await _taskService.CreateTask(task);

            var response = ApiResponse<CreateTaskItemRequest>.Created(task, "Task created succesfully");
            return CreatedAtAction(nameof(GetTask), new {id = createdTask.Id}, response);
        }

        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskItem task)
        {
            if (id != task.Id)
                return BadRequest(ApiResponse<object>.BadRequest("ID in route does not match ID in body."));

            var success = await _taskService.UpdateTask(id, task);

            return success ? NoContent() : NotFound(ApiResponse<object>.NotFound("Task not found"));

        }

        [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var success = await _taskService.DeleteTask(id);

            return success ? NoContent() : NotFound(ApiResponse<object>.NotFound("Task not found"));
        }
    }
}
