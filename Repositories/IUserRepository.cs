using NZWalks.Models.Dormain;

namespace NZWalks.Repositories
{
    public interface IUserRepository
    {
        Task<User> AuthenticationAsync(string username, string password);
    }
}
