using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories;

public class UserRepository : IUserRepository
{
    private readonly BackendDbContext _backendDbContext;

    public UserRepository(BackendDbContext backendDbContext)
    {
        _backendDbContext = backendDbContext;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _backendDbContext.Users.ToList();
    }

    public User GetUserById(Guid id)
    {
        return _backendDbContext.Users.FirstOrDefault(u => u.Id == id)!;
    }

    public User GetUserByFamilyId(Guid id)
    {
        return _backendDbContext.Users.FirstOrDefault(u => u.FamilyId == id)!;
    }

    public List<User> GetUsersByFamilyId(Guid id)
    {
        return _backendDbContext.Users.Where(u => u.FamilyId == id).ToList();
    }

    public User GetUserByEmail(User user)
    {
        var email = user.Email;
        return _backendDbContext.Users.FirstOrDefault(u => u.Email == email)!;
    }

    public IEnumerable<User> SearchUserByEmailOrName(string search)
    {
        IEnumerable<User> users = _backendDbContext.Users
            .Where(u => u.Email.Contains(search) || u.FirstName.Contains(search))
            .ToList();
        return users;
    }

    public void CreateUser(User user)
    {
        _backendDbContext.Users.Add(user);
        _backendDbContext.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        _backendDbContext.Users.Update(user);
        _backendDbContext.SaveChanges();
    }

    public void UpdateUserById(User user)
    {
        _backendDbContext.Users.Update(user);
        _backendDbContext.SaveChanges();
    }

    public void DeleteUserById(Guid id)
    {
        _backendDbContext.Users.Remove(GetUserById(id));
        _backendDbContext.SaveChanges();
    }

    public void DeleteUserByFamilyId(Guid id)
    {
        _backendDbContext.Users.Remove(GetUserByFamilyId(id));
        _backendDbContext.SaveChanges();
    }
}
