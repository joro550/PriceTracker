using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Users;

namespace Prices.Web.Server.Tests.Controllers
{
    public static class HttpClientExtenstion
    {
        private static async Task<HttpClient> AuthorizeWithAsync(this HttpClient client, TestUserEntity user)
        {
            var response = await client.PostAsJsonAsync("/api/user/login",
                new UserModel {Username = user.Username, Password = user.OriginalPassword});
            var tokenResponse = JsonConvert.DeserializeObject<TokenResult>(await response.Content.ReadAsStringAsync());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);
            return client;
        }

        public static HttpClient AuthorizeWith(this HttpClient client, TestUserEntity user)
        {
            Task.Run(async () => await AuthorizeWithAsync(client, user)).Wait();
            return client;
        }
    }
}