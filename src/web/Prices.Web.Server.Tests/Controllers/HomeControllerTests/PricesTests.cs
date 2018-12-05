using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Data;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Home;
using Xunit;

namespace Prices.Web.Server.Tests.Controllers.HomeControllerTests
{
    public class PricesTests
    {
        public class GivenItemsInTheDatabase : IClassFixture<HomeControllerTestFixture>
        {
            private readonly HomeControllerTestBuilder _builder;

            public GivenItemsInTheDatabase(HomeControllerTestFixture fixture) 
                => _builder = fixture.Builder;

            [Fact]
            public async Task WhenRequestingPricesView_ThenPricesAreInTheModel()
            {
                var fixture = new Fixture();
                var itemPrice = fixture
                    .Create<ItemPriceEntity>();

                var repository = new ItemPriceRepositoryWithSpecifiedPrices(new List<ItemPriceEntity> {itemPrice});
                var homeController = _builder
                    .WithItemPriceRepository(repository)
                    .BuildController();

                var viewResult = await homeController.Prices(itemPrice.PartitionKey) as ViewResult;
                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);

                var viewModel = viewResult.Model as ItemModel;
                Assert.NotNull(viewModel);
                Assert.Equal(itemPrice.PartitionKey, viewModel.Id);
                Assert.Single(viewModel.Prices.DataSets);
                Assert.Equal(itemPrice.PartitionKey, viewModel.Prices.DataSets[0].Label);
            }
        }

        public class GivenNoPricesInTheDatabase : IClassFixture<HomeControllerTestFixture>
        {
            private readonly HomeControllerTestBuilder _builder;

            public GivenNoPricesInTheDatabase(HomeControllerTestFixture fixture) 
                => _builder = fixture.Builder;

            [Fact]
            public async Task WhenRequestingPricesView_ThenModelHasEmptyCollection()
            {
                var homeController = _builder
                    .WithItemPriceRepository(new ItemPriceRepositoryWithNoPrices())
                    .BuildController();
                
                var viewResult = await homeController.Prices(string.Empty) as ViewResult;
                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);
                
                var viewModel = viewResult.Model as ItemModel;
                Assert.NotNull(viewModel);
                Assert.Empty(viewModel.Prices.DataSets);
                Assert.Empty(viewModel.Prices.Labels);
            }
        }
    }
}