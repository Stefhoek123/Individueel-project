using Models;
using Repositories;

namespace Interface
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        IEnumerable<User> GetAllUsers();
        User GetUserById(Guid id);
        User GetUserByEmail(string email);
        User GetUserByFamilyId(Guid id);
        List<User> GetUsersByFamilyId(Guid id);
        IEnumerable<User> SearchUserByEmailOrName(string search);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUserById(Guid id);
        void DeleteUserByFamilyId(Guid id);

    }
}
