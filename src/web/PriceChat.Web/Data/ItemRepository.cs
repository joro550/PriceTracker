using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PriceChat.Web.Data
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAll();
        Task<List<Item>> ByPartitionKey(string value);
        Task Add(Item item);
    }

    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(CloudTableClient client) 
            : base(client.GetTableReference("items"))
        {
        }

    }
}