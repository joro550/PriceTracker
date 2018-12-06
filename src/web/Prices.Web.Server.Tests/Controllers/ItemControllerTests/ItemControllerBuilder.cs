using AutoMapper;
using Moq;
using Prices.Web.Server.Controllers;
using Prices.Web.Server.Data;

namespace Prices.Web.Server.Tests.Controllers.ItemControllerTests
{
    public class ItemControllerBuilder
    {
        private readonly IMapper _mapper;
        private IItemRepository _itemRepository;

        public ItemControllerBuilder()
        {
            _itemRepository = new Mock<IItemRepository>().Object;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();
        }


        public ItemControllerBuilder WithItemRepository(IItemRepository repository)
        {
            _itemRepository = repository;
            return this;
        }

        public ItemController Build()
        {
            return new ItemController(_itemRepository, _mapper);
        }
    }
}