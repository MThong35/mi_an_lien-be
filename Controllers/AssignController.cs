using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.DTOs.RequestDTO;
using thesis_comicverse_webservice_api.Models;
using thesis_comicverse_webservice_api.Repositories;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignController : ControllerBase
    {

        private readonly ILogger<AssignController> _logger;
        private readonly IAssignment _assignmentRepository;

        public AssignController(ILogger<AssignController> logger, IAssignment assignmentRepository)
        {
            _logger = logger;
            _assignmentRepository = assignmentRepository;
        }

        [HttpGet("list-assign")]
        public async Task<IActionResult> GetAllAssignments()
        {
            try
            {
                _logger.LogInformation("Getting all assignments");
                var assignments = await _assignmentRepository.GetAllAssignmentsAsync();
                return Ok(assignments);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            try
            {
                var assignment = await _assignmentRepository.GetAssignmentByIdAsync(id);
                if (assignment == null)
                {
                    return NotFound();
                }
                return Ok(assignment);
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            try
            {
                var assignment = await _assignmentRepository.DeleteAssignmentAsync(id);
                if (assignment == null)
                {
                    return NotFound();
                }
                return Ok(assignment);
            }
            catch
            {
                throw;
            }
        }


        [HttpPost("add-task")]
        public async Task<IActionResult> AddAssignment([FromBody] Models.Task task)
        {
            try
            {
                if (task == null)
                {
                    return BadRequest();
                }
                var createdTask = await _assignmentRepository.AddTaskAsync(task);


                //var as
                return CreatedAtAction(nameof(GetAllAssignments), new { id = createdTask.taskID }, createdTask);
            }
            catch
            {
                throw;
            }
        }

        [HttpPut("update-task")]
        public async Task<IActionResult> UpdateAssignment([FromBody] Models.Task task)
        {
            try
            {
                var updatedTask = await _assignmentRepository.UpdateTaskAsync(task);
                if (updatedTask == null)
                {
                    return NotFound();
                }
                return Ok(updatedTask);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("list-task")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                _logger.LogInformation("Getting all assignments");
                var assignments = await _assignmentRepository.GetAllTaskAsync();
                return Ok(assignments);
            }
            catch
            {
                throw;
            }
        }

        [HttpPut("update-assign")]
        public async Task<IActionResult> AssignTaskForUser([FromBody] AssignUserTaskDTO request)
        {
            try
            {
                _logger.LogInformation("Assign User Task");
                var assignments = await _assignmentRepository.AssignUserTask(request.userID, request.taskID);
                return Ok(assignments);
            }
            catch
            {
                throw;
            }
        }
    }
}
