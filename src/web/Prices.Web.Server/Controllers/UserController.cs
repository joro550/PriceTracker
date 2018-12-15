using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Shared.Models.Users;

namespace Prices.Web.Server.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly ITokenService _tokenService;

        public UserController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [Route("GenerateToken")]
        public async Task<IActionResult> Login([FromBody]UserModel user)
        {
            var token = _tokenService.BuildToken(user.Username);
            return Ok(new {token});
        }
    }
}