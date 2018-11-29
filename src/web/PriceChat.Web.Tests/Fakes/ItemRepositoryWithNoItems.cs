using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriceChat.Web.Data;

namespace PriceChat.Web.Tests.Fakes
{
    public class ItemRepositoryWithNoItems : IItemRepository
    {
        private List<Item> Items { get; } = new List<Item>();

        public Task<List<Item>> GetAll() 
            => Task.FromResult(Items);

        public Task<List<Item>> ByPartitionKey(string value) 
            => Task.FromResult(Items.Where(item => item.PartitionKey == value).ToList());

        public Task Add(Item item)
            => Task.Run(() => Items.Add(item));
    }    
}