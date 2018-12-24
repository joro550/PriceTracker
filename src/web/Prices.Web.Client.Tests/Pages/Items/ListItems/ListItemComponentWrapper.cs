using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Prices.Web.Client.Pages.Items;
using Prices.Web.Shared.Models.Home;

namespace Prices.Web.Client.Tests.Pages.Items.ListItems
{
    public class ListItemComponentWrapper : ListItemComponent
    {
        public ListItemComponentWrapper(HttpClient client)
        {
            Client = client;
        }

        public List<Item> GetItems()
        {
            return Items;
        }

        public async Task InitAsync()
        {
            await OnInitAsync();
        }
    }
}