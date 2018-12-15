using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.WindowsAzure.Storage.Table;
using Prices.Web.Server.Data;
using Prices.Web.Server.Tests.Fakes;

namespace Prices.Web.Server.Tests.Controllers.UserControllerTests
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
    }
}