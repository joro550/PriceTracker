using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Handlers.Data
{
    public interface IItemPriceRepository
    {
        Task<List<ItemPriceEntity>> GetAll();
        Task<List<ItemPriceEntity>> ByPartitionKey(string value);
        Task<bool> Add(ItemPriceEntity item);
    }

    public class ItemPriceRepository : Repository<ItemPriceEntity>, IItemPriceRepository
    {
        public ItemPriceRepository(CloudTableClient client)
            : base(client.GetTableReference("prices"))
        {
        }
    }
}