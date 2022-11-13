using Microsoft.AspNetCore.Mvc;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;
        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            /*validate incoming request*/

            /* check if user is authenticated*/

            /*  check username and password*/
            var user = await userRepository.AuthenticationAsync(loginRequest.UserName,
                loginRequest.Password);

            if (user != null)
            {
                /* Generate a JWT Token*/ /*Gotten from Token handler*/
              var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }
            return BadRequest("Username or Password is incorrect");
        }
    }
}
