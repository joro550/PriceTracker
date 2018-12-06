using System.Collections.Generic;
using Prices.Web.Server.Data;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeItemRepository : BaseRepository<ItemEntity>, IItemRepository
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
            return new FakeItemRepository(Items);
        }

        public static FakeItemRepository WithNoItems()
        {
            return new FakeItemRepository(new List<ItemEntity>());
        }
    }
}