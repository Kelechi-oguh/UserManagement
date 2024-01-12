using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Utils;
using UserManagement.Dtos;
using UserManagement.Models;
using UserManagement.Services.UserService;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet("all-users"), Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            
            if (!users.Any())
            {
                return BadRequest(new response<List<User>>{ Code = "203", Message = "No record found"});
            }

            var r = new response<List<User>>{ Data = users, Code = "00", Message = "Users retrieved successfully"};
            return Ok(r);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{userId}"), Authorize]
        public IActionResult GetUser(int userId)
        {
            if (!_userService.UserExists(userId))
                return BadRequest("User does not exist");

            var user = _userService.GetUserById(userId);

            var r = new response<User>{ Data= user, Code="200", Message="User retrieved successfully" };
            return Ok(r);
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

            var r = new response<User>{ Data= user, Code="200", Message="User created successfully" };
            return Ok(r);
        }

        [HttpPost("login")]
        public IActionResult UserLogin([FromBody] loginDto request)
        {
            var user = _userService.GetAllUsers()
                .Where(u=> u.Username == request.Username)
                .FirstOrDefault();

            if (user == null)
                return BadRequest("User does not exist");

            var passwordIsValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!passwordIsValid)
                return BadRequest("Incorrect Password");


            if (user != null && passwordIsValid)
            {
                DateTime expireAt = DateTime.Now.AddMinutes(5);

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.UserData, user.Password),
                    //new Claim(ClaimTypes.Role, "Manager")

                };

                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: "issuer",
                    audience: "audience",
                    claims: claims,
                    expires: expireAt,
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                var authToken = new AuthToken { ExpireAt = $"{expireAt}", Token = tokenString };

                return Ok(new response<AuthToken> { Data = authToken, Code = StatsCodes.SUCCESS, Message = "Login Successful" });
            }
            return Unauthorized();

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
    