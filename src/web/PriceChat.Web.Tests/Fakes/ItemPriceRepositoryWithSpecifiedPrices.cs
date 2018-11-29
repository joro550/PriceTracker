using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriceChat.Web.Data;

namespace PriceChat.Web.Tests.Fakes
{
    public class ItemPriceRepositoryWithSpecifiedPrices : IItemPriceRepository
    {
        private readonly List<ItemPrice> _itemPrices;

        public ItemPriceRepositoryWithSpecifiedPrices(List<ItemPrice> itemPrices) 
            => _itemPrices = itemPrices;

        public Task<List<ItemPrice>> GetAll() 
            => Task.FromResult(_itemPrices);

        public Task<List<ItemPrice>> ByPartitionKey(string value) 
            => Task.FromResult(_itemPrices.Where(price => price.PartitionKey == value).ToList());

        public Task Add(ItemPrice item) 
            => Task.Run(() => _itemPrices.Add(item));
    }
}