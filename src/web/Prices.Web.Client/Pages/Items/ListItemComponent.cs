using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;
using Prices.Web.Shared.Models.Home;

namespace Prices.Web.Client.Pages.Items
{
    public class ListItemComponent : BlazorComponent
    {
        protected List<Item> Items { get; set; }

        protected override async Task OnInitAsync()
        {
            Items = new List<Item>{new Item {Id ="1", Retailer = "Amazon", Category = "Gaming"}};
        }
    }
}
