using System.Collections.Generic;
using System.Threading.Tasks;
using PriceChat.Web.Data;

namespace PriceChat.Web.Tests.Fakes
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