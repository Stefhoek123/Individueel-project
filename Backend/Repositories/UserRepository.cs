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

    public void DeleteUserById(Guid id)
    {
        _backendDbContext.Users.Remove(GetUserById(id));
        _backendDbContext.SaveChanges();
    }
}