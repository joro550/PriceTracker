using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Identity;
using Prices.Web.Shared.Models.Users;

namespace Prices.Web.Server.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly SignInManager<PriceWebUser> _signInManager;

        public UserController(SignInManager<PriceWebUser> signInManager) 
            => _signInManager = signInManager;

        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserModel user)
        {
            var identityUser = new PriceWebUser { UserName = user.Username, Password = user.Password};
            await _signInManager.SignInAsync(identityUser, true);
            return Ok();
        }
    }

    [Authorize, ApiController, Route("/api/thing")]
    public class ThingController : ControllerBase
    {
        public IActionResult Test()
        {
            return Ok("HelloWorld");
        }
    }

}