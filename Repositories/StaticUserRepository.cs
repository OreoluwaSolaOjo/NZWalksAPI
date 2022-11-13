using NZWalks.Models.Dormain;

namespace NZWalks.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
           /* new User()
            {
                FirstName = "Read Only",
                LastName = "User",
                EmailAddress = "readonly@user.com",
                Id = Guid.NewGuid(),
                UserName = "readonly@user.com",
                Password = "readonly@password",
                Roles = new List<string>{"reader"}
            },
              new User()
            {
                FirstName = "ReadWrite",
                LastName = "User",
                EmailAddress = "readwrite@user.com",
                Id = Guid.NewGuid(),
                UserName = "readwrite@user.com",
                Password = "readwrite@password",
                Roles = new List<string>{"reader", "writer"}
            },*/
        };
        public async Task<User> AuthenticationAsync(string username, string password)
        {
           var user = Users.Find(x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase)
            && x.Password == password);
            
            
            return user;
        }
    }
}
