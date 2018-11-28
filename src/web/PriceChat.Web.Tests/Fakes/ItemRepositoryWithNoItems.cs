using System.Collections.Generic;
using System.Threading.Tasks;
using PriceChat.Web.Data;

namespace PriceChat.Web.Tests.Fakes
{
    public class ItemRepositoryWithNoItems : IItemRepository
    {
        public Task<List<Item>> GetAll() 
            => Task.FromResult(new List<Item>());

        public Task<List<Item>> ByPartitionKey(string value) 
            => Task.FromResult(new List<Item>());

        public Task Add(Item item)
            => Task.CompletedTask;
    }
}