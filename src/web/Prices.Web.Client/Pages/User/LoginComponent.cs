using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Prices.Web.Shared.Models.Users;

namespace Prices.Web.Client.Pages.User
{
    public class LoginComponent : BlazorComponent
    {
        [Inject] protected HttpClient Client { private get; set; }
        [Inject] protected  ILogger<LoginComponent> Logger { get; set; }
        protected UserModel User { get; } = new UserModel();

        protected async Task Login()
        {
            var serialize = Json.Serialize(User);
            Logger.LogDebug(serialize);

            var stringContent = new StringContent(serialize, Encoding.UTF8, "application/json");
//            var response = await Client.PostAsync("/api/user/login", stringContent);
            //Logger.LogDebug(response.StatusCode.ToString());


            var response = await Client.GetAsync("/api/items/hello");
            Logger.LogDebug(response.StatusCode.ToString());

        }
    }
}