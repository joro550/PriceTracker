using Prices.Web.Server.Controllers;
using Prices.Web.Server.Data;
using Prices.Web.Server.Tests.Fakes;

namespace Prices.Web.Server.Tests.Controllers.PriceControllerTests
{
    public class PriceControllerBuilder
    {
        private IItemPriceRepository _itemPriceRepository;

        public PriceControllerBuilder WithItemPriceRepository(IItemPriceRepository priceRepository)
        {
            _itemPriceRepository = priceRepository;
            return this;
        }
        
        public PriceController Build()
        {
            var storageAccount = FakeStorageAccount.DevelopmentStorageAccount;
            var itemPriceRepository = _itemPriceRepository ?? new ItemPriceRepository(storageAccount.CreateCloudTableClient());
            return new PriceController(itemPriceRepository);
        }
        
    }
}