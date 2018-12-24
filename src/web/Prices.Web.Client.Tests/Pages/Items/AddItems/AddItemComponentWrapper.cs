using System.Threading.Tasks;
using BlazorState;
using MediatR;
using Microsoft.AspNetCore.Blazor.Services;
using Prices.Web.Client.Data;
using Prices.Web.Client.Pages.Items;

namespace Prices.Web.Client.Tests.Pages.Items.AddItems
{
    public class AddItemComponentWrapper : AddItemComponent
    {
        public AddItemComponentWrapper(IUriHelper helper, 
            IStore store,
            IMediator mediator)
        {
            UriHelper = helper;
            Store = store;
            Mediator = mediator;
        }

        public async Task AddUserToken(string token)
        {
            await Mediator.Send(new LoginRequest {UserToken = token});
        }

        public void Init() 
            => OnInit();
    }
}