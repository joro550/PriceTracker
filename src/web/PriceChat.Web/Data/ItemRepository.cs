using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PriceChat.Web.Data
{
    public interface IItemRepository
    {
        Task<List<ItemEntity>> GetAll();
        Task<List<ItemEntity>> ByPartitionKey(string value);
        Task Add(ItemEntity itemEntity);
    }

    public class ItemRepository : Repository<ItemEntity>, IItemRepository
    {
        public ItemRepository(CloudTableClient client) 
            : base(client.GetTableReference("items"))
        {
        }

    }
}