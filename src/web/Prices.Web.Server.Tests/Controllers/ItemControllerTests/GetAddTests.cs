using Microsoft.AspNetCore.Mvc;
using Prices.Web.Shared.Models.Items;
using Xunit;

namespace Prices.Web.Server.Tests.Controllers.ItemControllerTests
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
                
            var viewModel = result.Model as AddItemModel;
            Assert.NotNull(viewModel);
        }
    }
}