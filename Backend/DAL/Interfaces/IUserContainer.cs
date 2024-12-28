using Domain;
using Models;

namespace DAL.Interfaces;

public interface IUserContainer
{
    IEnumerable<UserDto> GetAllUsers();
    UserDto GetUserById(Guid id);
    UserDto GetUserByEmail(string email);
    UserDto GetUserByFamilyId(Guid id);
    List<UserDto> GetUsersByFamilyId(Guid id);
    Task<bool> AuthenticateUserAsync(string email, string password);
    Task<bool> IsAccountAvailableAsync(string email);
    IEnumerable<UserDto> SearchUserByEmailOrName(string search);
    void CreateUser(UserDto user);
    void UpdateUser(UserDto user);
    void UpdateUserById(Guid id, UserDto user);
    void DeleteUserById(Guid id);
    void DeleteUserByFamilyId(Guid id);
}