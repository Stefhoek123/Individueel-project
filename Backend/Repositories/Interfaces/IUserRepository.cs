using Models;
using Repositories;

namespace Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(Guid id);
        User GetUserByFamilyId(Guid id);
        List<User> GetUsersByFamilyId(Guid id);
        User GetUserByEmail(User user);
        IEnumerable<User> SearchUserByEmailOrName(string search);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUserById(Guid id);
        void DeleteUserByFamilyId(Guid id);

    }
}
