using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            UserDto user = _userContainer.GetUserById(id);
            return Ok(user);
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

        [HttpDelete(nameof(DeleteUserById))]
        public ActionResult DeleteUserById(Guid id)
        {
            _userContainer.DeleteUserById(id);
            return Ok();
        }
    }
}
