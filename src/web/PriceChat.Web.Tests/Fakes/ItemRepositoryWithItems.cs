using System.Collections.Generic;
using System.Linq;
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
            => Task.FromResult(Items.Where(item => item.PartitionKey == value).ToList());

        public Task Add(Item item) 
            => Task.Run(() => Items.Add(item));
    }
}