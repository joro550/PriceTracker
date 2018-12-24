using System.Collections.Generic;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeItemRepository : InMemoryRepository<ItemEntity>, IItemRepository
    {
        public static readonly List<ItemEntity> StandardItems = new List<ItemEntity>
        {
            new ItemEntity
            {
                Id = "1",
                Category = "Category",
                Retailer = "Retailer"
            }
        };

        private FakeItemRepository(List<ItemEntity> items)
            : base(items)
        {
        }

        public static FakeItemRepository WithItems(List<ItemEntity> items)
        {
            return new FakeItemRepository(items);
        }

        public static FakeItemRepository WithStandardItems()
        {
            return new FakeItemRepository(StandardItems);
        }

        public static FakeItemRepository WithNoItems()
        {
            return new FakeItemRepository(new List<ItemEntity>());
        }
    }
}