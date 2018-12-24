using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using BlazorState;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.Extensions.DependencyInjection;
using Prices.Web.Client.Pages.User;
using Prices.Web.Client.Tests.Fakes;

namespace Prices.Web.Client.Tests.Pages.Users
{
    public class LoginComponentBuilder
    {
        private HttpClient _client;
        private IUriHelper _uriHelper;

        public LoginComponentBuilder WithMessageHandler(HttpMessageHandler messageHandler)
        {
            _client = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            return this;
        }

        public LoginComponentBuilder WithUriHelper(IUriHelper helper)
        {
            _uriHelper = helper;
            return this;
        }
        
        public LoginComponentWrapper Build()
        {
            var client = _client ?? new HttpClient(FakeHttpMessageHandler.WithNotFoundResult())
            {
                BaseAddress = new Uri("http://localhost/")
            };
            
            var helper = _uriHelper ?? new FakeUriHelper();

            return new ServiceCollection()
                .AddBlazorState(options =>
                {
                    options.Assemblies = new List<Assembly> {typeof(LoginComponent).Assembly};
                })
                .AddSingleton(client)
                .AddSingleton(helper)
                .AddSingleton<LoginComponentWrapper>()
                .BuildServiceProvider()
                .GetService<LoginComponentWrapper>();
        }
    }
}