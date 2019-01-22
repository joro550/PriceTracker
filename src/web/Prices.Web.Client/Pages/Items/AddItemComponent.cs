using System.Threading.Tasks;
using BlazorState;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using Prices.Web.Client.Data;
using Prices.Web.Shared.Models.Items;

namespace Prices.Web.Client.Pages.Items
{
    public class AddItemComponent : BlazorStateComponent
    {
        [Inject] protected IUriHelper UriHelper { set; get; }

        protected AddItemModel ItemModel { get; set; } = new AddItemModel();

        protected override void OnInit()
        {
            var state = Store.GetState<UserState>();
            if(string.IsNullOrEmpty(state.Token))
                UriHelper.NavigateTo("/");
        }

        public Task AddItem()
        {
            return Task.CompletedTask;
        }
    }
}