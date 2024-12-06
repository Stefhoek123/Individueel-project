using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Dependency Injection of user container
        private readonly IUserContainer _userContainer;
        public UserController(IUserContainer userContainer)
        {
            _userContainer = userContainer;
        }

        [HttpGet(nameof(GetAllUsers))]
        public ActionResult<IEnumerable<UserDto>> GetAllUsers()
        {
            return Ok(_userContainer.GetAllUsers());
        }

        [HttpGet(nameof(GetUserById))]
        public ActionResult<UserDto> GetUserById(Guid id)
        {
            return Ok(_userContainer.GetUserById(id));
        }

        [HttpGet(nameof(GetUserByFamilyId))]
        public ActionResult<UserDto> GetUserByFamilyId(Guid id)
        {
            return Ok(_userContainer.GetUserByFamilyId(id));
        }

        [HttpGet(nameof(GetUsersByFamilyId))]
        public ActionResult<List<UserDto>> GetUsersByFamilyId(Guid id)
        {
            List<UserDto> users = _userContainer.GetUsersByFamilyId(id);
            if (users == null || users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto loginRequest)
        {
            if (await _userContainer.AuthenticateUserAsync(loginRequest.Email, loginRequest.PasswordHash))
            {
                return Ok(new { message = "Login successful" });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }

        [HttpPost("check-account")]
        public async Task<IActionResult> CheckAccount([FromBody] UserDto request)
        {
            if (await _userContainer.IsAccountAvailableAsync(request.Email))
            {
                return NotFound(new { message = "Account not found. Please register." });
            }

            return Ok(new { message = "Account exists" });
        }

        [HttpGet(nameof(SearchUserByEmailOrName))]
        public ActionResult<IEnumerable<UserDto>> SearchUserByEmailOrName(string? search)
        {
            if (search != null)
            {
                return Ok(_userContainer.SearchUserByEmailOrName(search));
            }

            return Ok(_userContainer.GetAllUsers());
        }

        [HttpPost(nameof(CreateUser))]
        public ActionResult CreateUser(UserDto user)
        {
            _userContainer.CreateUser(user);
            return Ok();
        }

        [HttpPut(nameof(UpdateUser))]
        public ActionResult UpdateUser(UserDto user)
        {
            _userContainer.UpdateUser(user);
            return Ok();
        }

        [HttpPut(nameof(UpdateUserById))]
        public ActionResult UpdateUserById(Guid id, UserDto user)
        {
            _userContainer.UpdateUserById(id, user);
            return Ok();
        }

        [HttpDelete(nameof(DeleteUserById))]
        public ActionResult DeleteUserById(Guid id)
        {
            _userContainer.DeleteUserById(id);
            return Ok();
        }

        [HttpDelete(nameof(DeleteUserByFamilyId))]
        public ActionResult DeleteUserByFamilyId(Guid id)
        {
            _userContainer.DeleteUserByFamilyId(id);
            return Ok();
        }

    }
}
