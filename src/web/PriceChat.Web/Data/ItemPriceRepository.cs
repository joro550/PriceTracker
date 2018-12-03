using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PriceChat.Web.Data
{
    public interface IItemPriceRepository
    {
        Task<List<ItemPriceEntity>> GetAll();
        Task<List<ItemPriceEntity>> ByPartitionKey(string value);
        Task Add(ItemPriceEntity item);
    }

    public class ItemPriceRepository : Repository<ItemPriceEntity>, IItemPriceRepository
    {
        public ItemPriceRepository(CloudTableClient client)
            : base(client.GetTableReference("prices"))
        {
        }
    }
}