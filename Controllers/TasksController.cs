using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleTaskManagerAPI.Models;
using SimpleTaskManagerAPI.Repositories;

namespace SimpleTaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskRepository repository, ILogger<TasksController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetById(int id)
        {
            var task = _repository.GetById(id);
            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public ActionResult<TaskItem> Add([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repository.Add(task);
            _logger.LogInformation("Task added: " + System.Text.Json.JsonSerializer.Serialize(task));

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TaskItem updatedTask)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_repository.Exists(updatedTask.Id))
            {
                _logger.LogWarning("Attempted to update non-existent task with id: {Id}", id);
                return NotFound();
            }

            updatedTask.Id = id;
            _repository.Update(updatedTask);
            _logger.LogInformation("Task updated: " + System.Text.Json.JsonSerializer.Serialize(updatedTask));

            return NoContent();
        }

        [HttpPatch("{id}/complete")]
        public IActionResult MarkAsComplete(int id)
        {
            if (!_repository.Exists(id))
            {
                _logger.LogWarning("Attempted to complete non-existent task with id: {Id}", id);
                return NotFound();
            }

            _repository.MarkComplete(id);
            _logger.LogInformation("Task marked complete: {Id}", id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_repository.Exists(id))
            {
                _logger.LogWarning("Attempted to delete non-existent task with id: {Id}", id);
                return NotFound();
            }

            _repository.Delete(id);
            _logger.LogInformation("Task deleted: {Id}", id);

            return NoContent();
        }
    }
}
