using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Identity;
using Prices.Web.Shared.Models.Users;
using Prices.Web.Server.Handlers.Requests;

namespace Prices.Web.Server.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        private readonly ICipherService _cipherService;

        public UserController(IMediator mediator, ITokenService tokenService, ICipherService cipherService)
        {
            _mediator = mediator;
            _tokenService = tokenService;
            _cipherService = cipherService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserModel user)
        {
            if (!await ValidateModel(user))
                return BadRequest();
            
            var userEntity = await _mediator.Send(new GetUserByUsernameRequest {Username = user.Username});
            if (_cipherService.ValidatePasswordAgainstHash(user.Password, userEntity.PasswordSalt, userEntity.Password))
                return Ok(new TokenResult { Token = _tokenService.BuildToken(user.Username)});
            return BadRequest();
        }

        private static async Task<bool> ValidateModel(UserModel user)
        {
            var validationResult = await new UserModelValidator()
                .ValidateAsync(user);
            return validationResult.IsValid;
        }
    }
}