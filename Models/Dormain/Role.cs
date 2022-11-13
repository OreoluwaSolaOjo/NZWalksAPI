using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NZWalks.Models.Dormain
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        //Navigation Property

        public List<User_Role> UserRoles { get; set; }
    }
}
