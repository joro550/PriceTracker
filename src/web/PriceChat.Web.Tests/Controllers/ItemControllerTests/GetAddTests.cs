using Microsoft.AspNetCore.Mvc;
using PriceChat.Web.Models.Items;
using Xunit;

namespace PriceChat.Web.Tests.Controllers.ItemControllerTests
{
    public class GetAddTests : IClassFixture<ItemControllerFixture>
    {
        private readonly ItemControllerBuilder _builder;

        public GetAddTests(ItemControllerFixture fixture)
            => _builder = fixture.Builder;

        [Fact]
        public void WhenAddIsCalled_AddViewIsReturned()
        {
            var itemController = _builder.Build();
            var result = Assert.IsType<ViewResult>(itemController.Add());
            Assert.NotNull(result);
                
            var viewModel = result.Model as ItemModel;
            Assert.NotNull(viewModel);
        }
    }
}