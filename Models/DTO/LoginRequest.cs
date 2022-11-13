using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Models.DTO
{
    public class LoginRequest
    {
       public string UserName { get; set; }
        public string Password { get; set; }
    }
}
