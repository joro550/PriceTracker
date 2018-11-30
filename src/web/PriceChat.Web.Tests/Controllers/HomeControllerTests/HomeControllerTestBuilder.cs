using AutoMapper;
using Moq;
using PriceChat.Web.Controllers;
using PriceChat.Web.Data;

namespace PriceChat.Web.Tests.Controllers.HomeControllerTests
{
    public class HomeControllerTestBuilder
    {
        private IItemRepository _itemRepository;
        private IItemPriceRepository _itemPriceRepository;
        private readonly IMapper _mapper;

        public HomeControllerTestBuilder()
        {
            _itemPriceRepository = new Mock<IItemPriceRepository>().Object;
            _itemRepository = new Mock<IItemRepository>().Object;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();
        }

        public HomeControllerTestBuilder WithItemRepository(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
            return this;
        }

        public HomeControllerTestBuilder WithItemPriceRepository(IItemPriceRepository itemPriceRepository)
        {
            _itemPriceRepository = itemPriceRepository;
            return this;
        }

        public HomeController BuildController() 
            => new HomeController(_itemRepository, _itemPriceRepository, _mapper);
    }
}