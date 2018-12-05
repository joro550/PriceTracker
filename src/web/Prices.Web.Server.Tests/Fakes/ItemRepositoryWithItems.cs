using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prices.Web.Server.Data;

namespace Prices.Web.Server.Tests.Fakes
{
    public class ItemRepositoryWithItems : IItemRepository
    {
        public static readonly List<ItemEntity> Items = new List<ItemEntity>
        {
            new ItemEntity
            {
                Id = "1",
                Category = "Category",
                Retailer = "Retailer"
            }
        };

        public Task<List<ItemEntity>> GetAll() 
            => Task.FromResult(Items);

        public Task<List<ItemEntity>> ByPartitionKey(string value)
            => Task.FromResult(Items.Where(item => item.PartitionKey == value).ToList());

        public Task Add(ItemEntity itemEntity) 
            => Task.Run(() => Items.Add(itemEntity));
    }
}