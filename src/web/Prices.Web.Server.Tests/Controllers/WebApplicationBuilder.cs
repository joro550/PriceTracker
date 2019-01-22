using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.TestHost;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Server.Handlers.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.WindowsAzure.Storage.Table;

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
            _serviceDescriptors.Add(
                ServiceDescriptor.Transient(typeof(IItemPriceRepository), sp => itemPriceRepository));
            return this;
        }
    }
}