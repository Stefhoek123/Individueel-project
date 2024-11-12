using Models;
using Repositories;

namespace Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(Guid id);
        User GetUserByEmail(User user);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUserById(Guid id);

    }
}
