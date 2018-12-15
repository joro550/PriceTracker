using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Data;
using Prices.Web.Server.Identity;
using Prices.Web.Shared.Models.Users;

namespace Prices.Web.Server.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public UserController(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserModel user)
        {
            var userEntity = await _userRepository.GetByUsername(user.Username);
            if (user.Password == userEntity?.Password)
                return Ok(new TokenResult { Token = _tokenService.BuildToken(user.Username)});
            return BadRequest();
        }
    }
}