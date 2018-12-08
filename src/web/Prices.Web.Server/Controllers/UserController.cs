using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Prices.Web.Server.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(SignInManager<IdentityUser> signInManager) 
            => _signInManager = signInManager;

//        [Route("Login")]
//        public async Task<IActionResult> Login(UserModel user)
//        {
//            var identityUser = new IdentityUser{ UserName = user.Username, PasswordHash = user.Password};
//            await _signInManager.SignInAsync(identityUser, true);
//            return Ok();
//        }

    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}