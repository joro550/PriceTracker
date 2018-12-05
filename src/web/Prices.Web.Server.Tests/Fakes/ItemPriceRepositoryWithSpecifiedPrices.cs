using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prices.Web.Server.Data;

namespace Prices.Web.Server.Tests.Fakes
{
    public class ItemPriceRepositoryWithSpecifiedPrices : IItemPriceRepository
    {
        private readonly List<ItemPriceEntity> _itemPrices;

        public ItemPriceRepositoryWithSpecifiedPrices(List<ItemPriceEntity> itemPrices) 
            => _itemPrices = itemPrices;

        public Task<List<ItemPriceEntity>> GetAll() 
            => Task.FromResult(_itemPrices);

        public Task<List<ItemPriceEntity>> ByPartitionKey(string value) 
            => Task.FromResult(_itemPrices.Where(price => price.PartitionKey == value).ToList());

        public Task Add(ItemPriceEntity item) 
            => Task.Run(() => _itemPrices.Add(item));
    }
}