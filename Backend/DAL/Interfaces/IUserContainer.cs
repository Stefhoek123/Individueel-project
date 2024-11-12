using Domain;
using Models;

namespace DAL.Interfaces;

public interface IUserContainer
{
    IEnumerable<UserDto> GetAllUsers();
    UserDto GetUserById(Guid id);
    UserDto GetUserByEmail(UserDto user);
    void CreateUser(UserDto user);
    void UpdateUser(UserDto user);
    void DeleteUserById(Guid id);
}