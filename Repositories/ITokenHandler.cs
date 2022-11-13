using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Dormain;

namespace NZWalks.Repositories
{
    public interface ITokenHandler 
    {
        Task<string> CreateTokenAsync(User user);
    }
}
