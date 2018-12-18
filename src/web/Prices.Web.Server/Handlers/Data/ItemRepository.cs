using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Handlers.Data
{
    public interface IItemRepository
    {
        Task<List<ItemEntity>> GetAll();
        Task<List<ItemEntity>> ByPartitionKey(string value);
        Task<bool> Add(ItemEntity itemEntity);
    }

    public class ItemRepository : Repository<ItemEntity>, IItemRepository
    {
        public ItemRepository(CloudTableClient client)
            : base(client.GetTableReference("items"))
        {
        }
    }
}