using DAL.Interfaces;
using Domain;
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
            UserDto user = _userContainer.GetUserById(id);
            return Ok(user);
        }

        [HttpGet(nameof(GetUserByFamilyId))]
        public ActionResult<UserDto> GetUserByFamilyId(Guid id)
        {
            UserDto user = _userContainer.GetUserByFamilyId(id);
            return Ok(user);
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

        [HttpGet(nameof(GetUserByEmail))]
        public ActionResult<UserDto> GetUserByEmail(UserDto user)
        {
            UserDto userEmail = _userContainer.GetUserByEmail(user);
            return Ok(userEmail);
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
