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
        private readonly ICipherService _cipherService;
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;

        public UserController(IMediator mediator, ITokenService tokenService, ICipherService cipherService)
        {
            _mediator = mediator;
            _tokenService = tokenService;
            _cipherService = cipherService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            var validationResult = await new UserModelValidator().ValidateAsync(user);
            if (!validationResult.IsValid)
                return BadRequest();

            var userEntity = await _mediator.Send(new GetUserByUsernameRequest {Username = user.Username});
            var validatePasswordAgainstHash =
                _cipherService.ValidatePasswordAgainstHash(user.Password, userEntity.PasswordSalt, userEntity.Password);

            if (validatePasswordAgainstHash)
                return Ok(new TokenResult {Token = _tokenService.BuildToken(user.Username)});
            return BadRequest();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel createUser)
        {
            var validationResult = new CreateUserValidator().Validate(createUser);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var userEntity = await _mediator.Send(new GetUserByUsernameRequest {Username = createUser.Username});
            if (userEntity.IsValidUser())
                return BadRequest();

            var passwordGenerationResult = _cipherService.GeneratePassword(createUser.Password);

            await _mediator.Send(new CreateUserRequest
            {
                Username = createUser.Username,
                Password = passwordGenerationResult.HashedPassword,
                PasswordSalt = passwordGenerationResult.PasswordSalt
            });
            return Ok();
        }
    }
}