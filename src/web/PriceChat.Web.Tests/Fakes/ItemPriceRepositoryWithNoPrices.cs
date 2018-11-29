using System.Collections.Generic;
using System.Threading.Tasks;
using PriceChat.Web.Data;

namespace PriceChat.Web.Tests.Fakes
{
    public class ItemPriceRepositoryWithNoPrices : IItemPriceRepository
    {
        public Task<List<ItemPrice>> GetAll() 
            => Task.FromResult(new List<ItemPrice>());

        public Task<List<ItemPrice>> ByPartitionKey(string value) 
            => Task.FromResult(new List<ItemPrice>());
    }
}