using Microsoft.WindowsAzure.Storage.Table;

namespace PriceChat.Web.Data
{
    public class ItemPriceRepository : Repository<ItemPrice>
    {
        public ItemPriceRepository(CloudTableClient client)
            : base(client.GetTableReference("prices"))
        {
        }
    }
}