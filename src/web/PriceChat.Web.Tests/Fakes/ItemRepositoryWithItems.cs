using System.Collections.Generic;
using System.Threading.Tasks;
using PriceChat.Web.Data;

namespace PriceChat.Web.Tests.Fakes
{
    public class ItemRepositoryWithItems : IItemRepository
    {
        public static readonly List<Item> Items = new List<Item>
        {
            new Item
            {
                Id = "1",
                Category = "Category",
                Retailer = "Retailer"
            }
        };

        public Task<List<Item>> GetAll() 
            => Task.FromResult(Items);

        public Task<List<Item>> ByPartitionKey(string value) 
            => throw new System.NotImplementedException();

        public Task Add(Item item)
        {
            Items.Add(item);
            return Task.CompletedTask;
        }
    }
}