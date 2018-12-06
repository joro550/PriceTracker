using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using Prices.Web.Shared.Models.Home;

namespace Prices.Web.Client.Pages.Items
{
    public class ListItemComponent : BlazorComponent
    {
        [Inject] protected HttpClient Client { private get; set; }

        protected List<Item> Items { get; private set; } = new List<Item>();

        protected override async Task OnInitAsync()
        {
            Items = await GetItems(await Client.GetAsync("/api/items"));
        }

        private static async Task<List<Item>> GetItems(HttpResponseMessage responseMessage)
        {
            return responseMessage.IsSuccessStatusCode
                ? await FromClientContent(responseMessage)
                : new List<Item>();
        }

        private static async Task<List<Item>> FromClientContent(HttpResponseMessage responseMessage)
        {
            return Json.Deserialize<List<Item>>(await responseMessage.Content.ReadAsStringAsync());
        }
    }
}