using Domain;
using Models;

namespace DAL.Interfaces;

public interface IUserContainer
{
    IEnumerable<UserDto> GetAllUsers();
    UserDto GetUserById(Guid id);
    UserDto GetUserByFamilyId(Guid id);
    List<UserDto> GetUsersByFamilyId(Guid id);
    UserDto GetUserByEmail(UserDto user);
    IEnumerable<UserDto> SearchUserByEmailOrName(string search);
    void CreateUser(UserDto user);
    void UpdateUser(UserDto user);
    void UpdateUserById(Guid id, UserDto user);
    void DeleteUserById(Guid id);
    void DeleteUserByFamilyId(Guid id);
}