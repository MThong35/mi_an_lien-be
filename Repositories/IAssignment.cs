using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using thesis_comicverse_webservice_api.Database;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Repositories
{
    public interface IAssignment
    {
        Task<IEnumerable<object>> GetAllAssignmentsAsync();
        Task<Models.Task> DeleteAssignmentAsync(int taskId);
        Task<Models.Task> AddTaskAsync(Models.Task task);
        Task<List<Models.Task>> GetAllTaskAsync();
        Task<Assign> AssignUserTask(int userId, int taskId);
        Task<Models.Task> UpdateTaskAsync(Models.Task task);
        Task<Models.Task> GetAssignmentByIdAsync(int id);
    }

    public class AssignmentRepository : IAssignment
    {
        private readonly AppDbContext _context;

        private readonly ILogger<AssignmentRepository> _logger;
        public AssignmentRepository(AppDbContext context, ILogger<AssignmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<object>> GetAllAssignmentsAsync()
        {
            try
            {
                if (_context.Assign == null)
                    throw new ArgumentNullException(nameof(_context.Assign));
                if (_context.Task == null)
                    throw new ArgumentNullException(nameof(_context.Task));
                if (_context.Users == null)
                    throw new ArgumentNullException(nameof(_context.Users));

                var assignments = await (
                    from task in _context.Task
                    join assign in _context.Assign
                    on task.taskID equals assign.TaskID into taskAssign
                    from assign in taskAssign.DefaultIfEmpty() // Left join with Assign table
                    join user in _context.Users
                    on assign.userId equals user.userId into assignUser
                    from user in assignUser.DefaultIfEmpty() // Left join with User table
                    select new
                    {
                        TaskId = task.taskID,
                        TaskName = task.taskName,
                        CreatedAt = task.createAt,
                        Deadline = task.deadline,
                        Progress = task.progressPercentage,
                        Priority = task.priority,
                        Status = task.status,
                        AssignedUserId = assign != null ? assign.userId : (int?)null, // Null check
                        AssignedAt = assign != null ? assign.assignAt : (DateTime?)null, // Null check
                        UserName = user != null ? user.userName : null, // Null check
                        UserEmail = user != null ? user.email : null, // Null check
                        UserRole = user != null ? user.role : null, // Null check
                        UserFirstName = assign != null && user != null ? user.firstName : null, // Include firstName if assigned
                        UserLastName = assign != null && user != null ? user.lastName : null  // Include lastName if assigned
                    }
                ).ToListAsync();

                return assignments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve assignments: {ex.Message}");
            }
        }

        public async Task<Models.Task> GetAssignmentByIdAsync(int id)
        {
            try
            {
                var task = await _context.Task!
                    .FirstOrDefaultAsync(t => t.taskID == id);

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve assignment: {ex.Message}");
            }
        }



        public async Task<Models.Task> DeleteAssignmentAsync(int taskId)
        {
            try
            {
                // Retrieve the assignments related to the TaskID
                var taskAssignments = await _context.Assign!
                    .Where(a => a.TaskID == taskId)
                    .ToListAsync();

                _logger.LogInformation($"TaskAssignments: {JsonConvert.SerializeObject(taskAssignments)}");

                if (taskAssignments != null || !taskAssignments!.Any())
                {
                    _context.Assign!.RemoveRange(taskAssignments!);
                }

                var task = await _context.Task!
                    .FirstOrDefaultAsync(t => t.taskID == taskId);

                if (task != null)
                {
                    _context.Task.Remove(task);
                }

                await _context.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete assignment and task: {ex.Message}");
            }
        }




        public async Task<Models.Task> AddTaskAsync(Models.Task task)
        {
            try
            {
                if (task == null)
                {
                    throw new ArgumentNullException(nameof(task));
                }

                await _context.Task!.AddAsync(task);
                await _context.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't add assignment: {ex.Message}");
            }
        }

        public async Task<Models.Task> UpdateTaskAsync(Models.Task task)
        {
            try
            {
                if (task == null)
                {
                    throw new ArgumentNullException(nameof(task));
                }

                _context.Task!.Update(task);
                await _context.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update assignment: {ex.Message}");
            }
        }

        public async Task<List<Models.Task>> GetAllTaskAsync()
        {
            try
            {
                var task = await _context.Task.ToListAsync();
                return task;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve task: {ex.Message}");
            }
        }

        public async Task<Assign> AssignUserTask(int userId, int taskId)
        {
            try
            {
                var assign = new Assign
                {
                    userId = userId,
                    TaskID = taskId,
                    assignAt = DateTime.Now
                };

                await _context.Assign!.AddAsync(assign);
                await _context.SaveChangesAsync();

                return assign;

            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't assign user to task: {ex.Message}");
            }
        }
    }
}