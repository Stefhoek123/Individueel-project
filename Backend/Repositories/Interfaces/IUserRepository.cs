using Models;

namespace Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(Guid Id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUserById(Guid Id);

    }
}
