using System.Collections.Generic;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeItemPriceRepository : InMemoryRepository<ItemPriceEntity>, IItemPriceRepository
    {
        private FakeItemPriceRepository(List<ItemPriceEntity> itemsPrices)
            : base(itemsPrices)
        {
        }

        public static FakeItemPriceRepository WithPrices(List<ItemPriceEntity> itemPrices)
        {
            return new FakeItemPriceRepository(itemPrices);
        }

        public static FakeItemPriceRepository WithNoPrices()
        {
            return new FakeItemPriceRepository(new List<ItemPriceEntity>());
        }
    }
}