using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PriceChat.Web.Data
{
    public interface IItemPriceRepository
    {
        Task<List<ItemPrice>> GetAll();
        Task<List<ItemPrice>> ByPartitionKey(string value);
        Task Add(ItemPrice item);
    }

    public class ItemPriceRepository : Repository<ItemPrice>, IItemPriceRepository
    {
        public ItemPriceRepository(CloudTableClient client)
            : base(client.GetTableReference("prices"))
        {
        }
    }
}