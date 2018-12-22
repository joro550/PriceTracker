using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Prices.Web.Server.Tests.Fakes;
using Microsoft.AspNetCore.TestHost;
using Prices.Web.Server.Handlers.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Prices.Web.Shared.Models.Users;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Tests.Controllers
{
    public class WebApplicationBuilder
    {
        private readonly List<ServiceDescriptor> _serviceDescriptors = new List<ServiceDescriptor>();

        public WebApplicationBuilder()
        {
            _serviceDescriptors.Add(ServiceDescriptor.Transient(typeof(CloudTableClient),
                sp => FakeStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient()));
        }

        public HttpClient Build()
        {
            return new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(config =>
                {
                    config.ConfigureTestServices(services =>
                    {
                        foreach (var serviceDescriptor in _serviceDescriptors)
                            services.Replace(serviceDescriptor);
                    });
                })
                .CreateClient();
        }

        public WebApplicationBuilder WithUserRepository(IUserRepository userRepository)
        {
            _serviceDescriptors.Add(ServiceDescriptor.Transient(typeof(IUserRepository), sp => userRepository));
            return this;
        }

        public WebApplicationBuilder WithItemRepository(IItemRepository itemRepository)
        {
            _serviceDescriptors.Add(ServiceDescriptor.Transient(typeof(IItemRepository), sp => itemRepository));
            return this;
        }

        public WebApplicationBuilder WithItemPriceRepository(IItemPriceRepository itemPriceRepository)
        {
            _serviceDescriptors.Add(ServiceDescriptor.Transient(typeof(IItemPriceRepository), sp => itemPriceRepository));
            return this;
        }
    }

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