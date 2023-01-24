using APIProject_AbuUllah_2111755.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProject_AbuUllah_2111755.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("CreatedBy/{token}")]
        public ActionResult<IEnumerable<Models.Task>> CreatedBy(string token)
        {
            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);
            if (session == null)
            {
                return NotFound("Token is invalid!");
            }

            var user = _context.Users.FirstOrDefault(x => x.Email == session.Email);

            var tasks = _context.Tasks.Where(x => x.CreatedByUid == user.Uid).ToList();

            if (tasks.Count == 0 || tasks is null)
            {
                return NotFound("There are no tasks created by this user!");
            }
            return tasks;
        }

        [HttpGet("AssignedTo/{token}")]
        public ActionResult<IEnumerable<Models.Task>> AssignedTo(string token)
        {
            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);
            if (session == null)
            {
                return NotFound("Token is invalid!");
            }

            var user = _context.Users.FirstOrDefault(x => x.Email == session.Email);

            var tasks = _context.Tasks.Where(x => x.AssignedToUid == user.Uid).ToList();

            if (tasks.Count == 0 || tasks is null)
            {
                return NotFound("There are no tasks created by this user!");
            }
            return tasks;
        }

        [HttpPost]
        public ActionResult Post(Dictionary<string, string> taskFromBody)
        {
            var assignedToName = _context.Users.FirstOrDefault(x => x.Uid == taskFromBody["AssignedToUid"]).Name;
            if (assignedToName is null)
            {
                return NotFound("AssignedToUid is invalid!");
            }

            var token = _context.Sessions.FirstOrDefault(x => x.Token == taskFromBody["Token"]);
            if (token is null)
            {
                return NotFound("Token's invalid!");
            }
            var usersEmail = token.Email;
            
            var createdByUser = _context.Users.FirstOrDefault(x => x.Email == usersEmail);

            var task = new Models.Task()
            { 
                CreatedByName = createdByUser.Name,
                CreatedByUid = createdByUser.Uid,
                AssignedToName = assignedToName,
                AssignedToUid = taskFromBody["AssignedToUid"],
                Description = taskFromBody["Description"]
            };

            _context.Tasks.Add(task);
            try
            {
                _context.SaveChanges();
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(task);
        }

        [HttpDelete("{taskUid}")]
        public ActionResult Delete(Dictionary<string, string> Token, string taskUid)
        {
            var token = Token["Token"];
            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);
            if (session is null)
            {
                return BadRequest("Token is invalid!");
            }
            var user = _context.Users.FirstOrDefault(x => x.Email == session.Email);
           
            var task = _context.Tasks.FirstOrDefault(x => x.TaskUid == taskUid);
            if (task is null)
            {
                return NotFound("Task Not Found.");
            }

            if (task.CreatedByUid != user.Uid)
            {
                return BadRequest("You do not have permission to delete this task.");
            }

            _context.Tasks.Remove(task);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(task);
        }

        [HttpPut("{taskUid}")]
        public ActionResult Put(Dictionary<string, object> updatedTask, string taskUid)
        {
            var token = updatedTask["Token"].ToString();
            var done = updatedTask["Done"].ToString();

            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);
            if (session is null)
            {
                return BadRequest("Token is invalid!");
            }
            var user = _context.Users.FirstOrDefault(x => x.Email == session.Email);

            var task = _context.Tasks.FirstOrDefault(x => x.TaskUid == taskUid);
            if (task is null)
            {
                return NotFound("Task Not Found.");
            }

            if (task.AssignedToUid != user.Uid)
            {
                return BadRequest("You do not have permission to update this task.");
            }

            task.Done = Convert.ToBoolean(done);
            _context.Tasks.Entry(task).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(task);
        }

    }
}
