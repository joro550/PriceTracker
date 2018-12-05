using System.Collections.Generic;
using System.Threading.Tasks;
using Prices.Web.Server.Data;

namespace Prices.Web.Server.Tests.Fakes
{
    public class ItemPriceRepositoryWithNoPrices : IItemPriceRepository
    {
        public Task<List<ItemPriceEntity>> GetAll() 
            => Task.FromResult(new List<ItemPriceEntity>());

        public Task<List<ItemPriceEntity>> ByPartitionKey(string value) 
            => Task.FromResult(new List<ItemPriceEntity>());

        public Task Add(ItemPriceEntity item) 
            => Task.CompletedTask;
    }
}