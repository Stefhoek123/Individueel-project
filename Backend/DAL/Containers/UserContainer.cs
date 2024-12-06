using DAL.Interfaces;
using Domain;
using Interface;
using Models;
using Repositories;

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

    public List<UserDto> GetUsersByFamilyId(Guid id)
    {
        var users = _userRepository.GetUsersByFamilyId(id);

        return users.Select(u => Mappers.UserMapper.ToDto(u)).ToList();
    }

    public UserDto GetUserByEmail(UserDto user)
    {
        var userdto = Mappers.UserMapper.ToModel(user);
        return Mappers.UserMapper.ToDto(_userRepository.GetUserByEmail(userdto));
    }

    // GET for finding a User based on his Email or Name
    public IEnumerable<UserDto> SearchUserByEmailOrName(string search)
    {
        return string.IsNullOrEmpty(search) ? GetAllUsers() : _userRepository.SearchUserByEmailOrName(search).Select(Mappers.UserMapper.ToDto);
    }

    // POST Creation of user
    public void CreateUser(UserDto user)
    {
        var userMapper = Mappers.UserMapper.ToModel(user);
        userMapper.Id = Guid.NewGuid();
        _userRepository.CreateUser(userMapper);
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

    public void DeleteUserByFamilyId(Guid id)
    {
        _userRepository.DeleteUserByFamilyId(id);
    }
}