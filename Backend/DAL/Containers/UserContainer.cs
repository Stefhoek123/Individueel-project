using DAL.Interfaces;
using Domain;
using Interface;

namespace DAL.Containers;

public class UserContainer : IUserContainer
{
    // Dependency injection of user repository
    private readonly IUserRepository _userRepository;
    public UserContainer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET for getting all users
    public IEnumerable<UserDto> GetAllUsers()
    {
        return _userRepository.GetAllUsers()
            .Select(Mappers.UserMapper.ToDto);
    }

    // GET for getting a user based on a User ID
    public UserDto GetUserById(Guid id)
    {
        return Mappers.UserMapper.ToDto(_userRepository.GetUserById(id));
    }

    // POST Creation of user
    public void CreateUser(UserDto userdto)
    {
        var user = Mappers.UserMapper.ToModel(userdto);
        user.Id = Guid.NewGuid();
        _userRepository.CreateUser(user);
    }

    // PUT for updating a user
    public void UpdateUser(UserDto user)
    {
        _userRepository.UpdateUser(Mappers.UserMapper.ToModel(user));
    }

    // DELETE of a user op basis van user ID 
    public void DeleteUserById(Guid id)
    {
        _userRepository.DeleteUserById(id);
    }
}