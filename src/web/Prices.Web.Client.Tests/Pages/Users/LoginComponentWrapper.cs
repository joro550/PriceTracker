using System.Net.Http;
using BlazorState;
using MediatR;
using Microsoft.AspNetCore.Blazor.Services;
using Prices.Web.Client.Data;
using Prices.Web.Client.Pages.User;

namespace Prices.Web.Client.Tests.Pages.Users
{
    public class LoginComponentWrapper : LoginComponent
    {
        public LoginComponentWrapper(HttpClient client, 
            IUriHelper uriHelper, 
            IMediator mediator, 
            IStore store)
        {
            Client = client;
            Mediator = mediator;
            Store = store;
            UriHelper = uriHelper;
        }

        public string GetUserToken() 
            => Store.GetState<UserState>().Token;
    }
}