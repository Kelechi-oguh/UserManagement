using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Dtos;
using UserManagement.Models;
using UserManagement.Services.UserService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagement.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<ValuesController>
        [HttpGet("all-users")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            if (!_userService.UserExists(userId))
                return NotFound();

            var user = _userService.GetUserById(userId);
            return Ok(user);
        }

        // POST api/<ValuesController>
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            if (!_userService.EmailIsValid(user.Email))
                return BadRequest("Email is not valid");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = _userService.GetUserByEmail(user.Email);

            if (userExists != null)
                return Ok("User with email already exists");

            var userCreated = _userService.CreateUser(user);
            if (!userCreated)
            {
                ModelState.AddModelError("", "Something went wrong creating the user");
                return BadRequest(ModelState);
            }
            return Created("User created sucessfully", user);
        }

        [HttpPost("login")]
        public IActionResult UserLogin([FromBody] loginDto details)
        {
            var userExists = _userService.GetAllUsers()
                .Where(u=> u.Username == details.Username)
                .FirstOrDefault();

            if (userExists == null)
                return BadRequest("User does not exist");

            var passwordIsValid = BCrypt.Net.BCrypt.Verify(details.Password, userExists.Password);

            if (!passwordIsValid)
                return BadRequest("Incorrect Password");

            return Ok("Login Sucessful");

        }


        // PUT api/<ValuesController>/5
        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] User request)
        {
            if (!_userService.UserExists(userId))
                return BadRequest("User Does not exist");

            var userUpdated = _userService.UpdateUser(userId, request);
            if (!userUpdated)
            {
                ModelState.AddModelError("", "Something went wrong when updating the user");
                return BadRequest(ModelState);
            }
            return Ok("User updated sucessfully");

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userService.UserExists(userId))
                return BadRequest("User does not exist");

            var userDeleted = _userService.DeleteUser(userId);

            if (!userDeleted)
            {
                ModelState.AddModelError("", "Something went wrong when deleting the user");
                return BadRequest(ModelState);
            }
            return Ok("User Deleted");
        }
    }
}
