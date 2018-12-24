using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using BlazorState;
using FluentValidation.Results;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Prices.Web.Client.Pages.Items;
using Prices.Web.Client.Tests.Fakes;

namespace Prices.Web.Client.Tests.Pages.Items.AddItems
{
    public class AddItemComponentBuilder
    {
        private IUriHelper _helper;
        private HttpClient _client;

        public AddItemComponentBuilder WithMessageHandler(HttpMessageHandler messageHandler)
        {
            _client = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            return this;
        }
        
        public AddItemComponentBuilder WithMessageHandlerWithValidationErrors(List<ValidationFailure> failure)
        {
            var stringContent = new StringContent(Json.Serialize(failure));
            var messageHandler = FakeHttpMessageHandler.WithBadRequestResponse(stringContent);
            _client = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            return this;
        }

        
        public AddItemComponentBuilder WithUriHelper(IUriHelper uriHelper)
        {
            _helper = uriHelper;
            return this;
        }
        
        public AddItemComponentWrapper Build()
        {
            var uriHelper = _helper ?? new FakeUriHelper();
            var client = _client ?? new HttpClient(FakeHttpMessageHandler.WithNotFoundResult())
            {
                BaseAddress = new Uri("http://localhost/")
            };
            
            return new ServiceCollection()
                .AddBlazorState(options =>
                {
                    options.Assemblies = new List<Assembly> {typeof(AddItemComponent).Assembly};
                })
                .AddSingleton(uriHelper)
                .AddSingleton(client)
                .AddSingleton<AddItemComponentWrapper>()
                .BuildServiceProvider()
                .GetService<AddItemComponentWrapper>();
        }
    }
}