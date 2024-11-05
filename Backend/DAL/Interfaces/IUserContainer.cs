using Domain;

namespace DAL.Interfaces;

public interface IUserContainer
{
    IEnumerable<UserDto> GetAllUsers();
    UserDto GetUserById(Guid id);
    void CreateUser(UserDto user);
    void UpdateUser(UserDto user);
    void DeleteUserById(Guid id);
}