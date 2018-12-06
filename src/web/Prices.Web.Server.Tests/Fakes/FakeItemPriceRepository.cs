using System.Collections.Generic;
using Prices.Web.Server.Data;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeItemPriceRepository : BaseRepository<ItemPriceEntity>, IItemPriceRepository
    {
        private FakeItemPriceRepository(List<ItemPriceEntity> itemPrices)
            : base(itemPrices)
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