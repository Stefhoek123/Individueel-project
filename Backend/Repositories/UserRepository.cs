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

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _backendDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            
            //SingleOrDefaultAsync(u => u.Email == email);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _backendDbContext.Users.ToList();
    }

    public User GetUserById(Guid id)
    {
        return _backendDbContext.Users.FirstOrDefault(u => u.Id == id)!;
    }

    public User GetUserByEmail(string email)
    {
        return _backendDbContext.Users.FirstOrDefault(u => u.Email == email)!;
    }


    public User GetUserByFamilyId(Guid id)
    {
        return _backendDbContext.Users.FirstOrDefault(u => u.FamilyId == id)!;
    }

    public List<User> GetUsersByFamilyId(Guid id)
    {
        return _backendDbContext.Users.Where(u => u.FamilyId == id).ToList();
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
