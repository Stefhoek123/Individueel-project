using Domain;
using Models;

namespace DAL.Interfaces;

public interface IUserContainer
{
    IEnumerable<UserDto> GetAllUsers();
    UserDto GetUserById(Guid id);
    UserDto GetUserByFamilyId(Guid id);
    UserDto GetUserByEmail(UserDto user);
    IEnumerable<UserDto> SearchUserByEmailOrName(string search);
    void CreateUser(UserDto user);
    void UpdateUser(UserDto user);
    void DeleteUserById(Guid id);
}