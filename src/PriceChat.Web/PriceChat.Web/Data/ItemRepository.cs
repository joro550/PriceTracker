using Microsoft.WindowsAzure.Storage.Table;

namespace PriceChat.Web.Data
{
    public class ItemRepository : Repository<Item>
    {
        public ItemRepository(CloudTableClient client) 
            : base(client.GetTableReference("items"))
        {
        }
    }
}