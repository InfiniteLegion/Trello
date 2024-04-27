using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Classes;

namespace Trello.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private CheloDbContext db;

        public TaskController(CheloDbContext db) { this.db = db; }

        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTaskById(int id)
        {
            Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                return BadRequest("Task not found");
            }

            return new ObjectResult(task);
        }

        [HttpPost]
        public async Task<ActionResult<Task>> AddNewTask(Task task)
        {
            if (task == null)
            {
                return BadRequest("Task not found");
            }

            await db.Tasks.AddAsync(task);
            await db.SaveChangesAsync();
            return Ok(task);
        }

        [HttpPut]
        public async Task<ActionResult<Task>> UpdateTask(Task task)
        {
            if (task == null)
            {
                return BadRequest("Task is null");
            }
            if (!db.Tasks.Any(x => x.Id == task.Id))
            {
                return BadRequest("Task not found");
            }

            Task originalTask = await db.Tasks.FirstOrDefaultAsync(x=>x.Id== task.Id);

            TaskValidator.CheckTaskUpdate(task, originalTask);
            await db.SaveChangesAsync();
            return Ok(originalTask);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskById(int id)
        {
            Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                return BadRequest("Task not found");
            }

            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
            return Ok("Task deleted");
        }

        [HttpGet("{id}/change-complete-status")]
        public async Task<ActionResult> ChangeTaskCompleteStatus(int id)
        {
            Task task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                return BadRequest("Task not found");
            }

            task.Iscompleted = !task.Iscompleted;
            await db.SaveChangesAsync();
            return Ok("Task complete status changed");
        }
    }
}
