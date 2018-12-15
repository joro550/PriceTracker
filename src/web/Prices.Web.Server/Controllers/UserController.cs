using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Handlers.Requests;
using Prices.Web.Server.Identity;
using Prices.Web.Shared.Models.Users;

namespace Prices.Web.Server.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;
        private readonly ICipherService _cipherService;

        public UserController(ITokenService tokenService, IMediator mediator, ICipherService cipherService)
        {
            _tokenService = tokenService;
            _mediator = mediator;
            _cipherService = cipherService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserModel user)
        {
            var userEntity = await _mediator.Send(new GetUserByUsernameRequest {Username = user.Username});
            var password = _cipherService.Encrypt(user.Password);

            if (user.Password == userEntity?.Password)
                return Ok(new TokenResult { Token = _tokenService.BuildToken(user.Username)});
            return BadRequest();
        }
    }
}