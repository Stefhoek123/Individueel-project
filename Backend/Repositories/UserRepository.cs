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
        return _backendDbContext.Users
            .Include(u => u.FirstName)
            .Include(u => u.LastName)
            .Include(u => u.Email)
            .ToList();
    }

    public User GetUserById(Guid Id)
    {
        return _backendDbContext.Users
            .Include(u => u.FirstName)
            .Include(u => u.LastName)
            .Include(u => u.Email)
            .FirstOrDefault(u => u.Id == Id)!;
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

    public void DeleteUserById(Guid Id)
    {
        _backendDbContext.Users.Remove(GetUserById(Id));
        _backendDbContext.SaveChanges();
    }
}