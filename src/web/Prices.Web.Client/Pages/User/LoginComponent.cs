using System;
using BlazorState;
using System.Net.Http;
using System.Threading.Tasks;
using Prices.Web.Client.Data;
using Microsoft.AspNetCore.Blazor;
using Prices.Web.Shared.Models.Users;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.AspNetCore.Blazor.Components;

namespace Prices.Web.Client.Pages.User
{
    public class LoginComponent : BlazorStateComponent
    {
        [Inject] protected HttpClient Client { private get; set; }
        [Inject] protected IUriHelper UriHelper { get; set; }
        
        public bool ShowLoginErrors;
        protected UserModel User { get; } = new UserModel();

        public async Task Login()
        {
            try
            {
                var result = await Client.PostJsonAsync<TokenResult>("/api/user/login", User);
                await Mediator.Send(new LoginRequest {UserToken = result.Token});
                UriHelper.NavigateTo("/");
            }
            catch (Exception)
            {
                ShowLoginErrors = true;
            }
        }
    }
}