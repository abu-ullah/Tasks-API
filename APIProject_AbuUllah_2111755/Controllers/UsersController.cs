using APIProject_AbuUllah_2111755.Data;
using APIProject_AbuUllah_2111755.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProject_AbuUllah_2111755.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Post(Dictionary<string, string> userFromBody)
        {
            if (userFromBody.Count == 2) // Logging In
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == userFromBody["Email"]);

                if (user is null || user.Password != userFromBody["Password"])
                {
                    return NotFound("Invalid email or password!");
                }

                var session = new Session(userFromBody["Email"]);
                _context.Sessions.Add(session);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok(session.Token);
            }
            else  // Creating User
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == userFromBody["Email"]);

                if (user != null)
                {
                    return BadRequest("This User already exists!");
                }

                user = new Models.User()
                {
                  Name = userFromBody["Name"], 
                  Email = userFromBody["Email"], 
                  Password = userFromBody["Password"] 
                };
                _context.Users.Add(user);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok(user);
            }
        }

    }
}
