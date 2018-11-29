using AutoMapper;
using Moq;
using PriceChat.Web.Controllers;
using PriceChat.Web.Data;

namespace PriceChat.Web.Tests.Controllers.ItemControllerTests
{
    public class ItemControllerBuilder
    {
        private IItemRepository _itemRepository;
        private IMapper _mapper;

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
            => new ItemController(_itemRepository, _mapper);
    }
}