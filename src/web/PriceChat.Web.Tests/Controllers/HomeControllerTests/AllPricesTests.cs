using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PriceChat.Web.Models;
using PriceChat.Web.Models.Home;
using PriceChat.Web.Tests.Fakes;
using Xunit;
using ItemPrice = PriceChat.Web.Data.ItemPrice;

namespace PriceChat.Web.Tests.Controllers.HomeControllerTests
{
    public class AllPricesTests
    {
        public class GivenNoItemsInTheDatabase : IClassFixture<HomeControllerTestFixture>
        {
            private readonly HomeControllerTestBuilder _builder;

            public GivenNoItemsInTheDatabase(HomeControllerTestFixture fixture) 
                => _builder = fixture.Builder;

            [Fact]
            public async Task WhenGettingAllPrices_ThenEmptyChartDataIsReturned()
            {
                var homeController = _builder
                    .WithItemPriceRepository(new ItemPriceRepositoryWithNoPrices())
                    .BuildController();
                
                var viewResult = await homeController.AllPrices() as ViewResult;

                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);

                var chartData = viewResult.Model as ChartData;
                Assert.NotNull(chartData);
                Assert.Empty(chartData.Labels);
                Assert.Empty(chartData.DataSets);
            }
        }

        public class GivenPricesInTheDatabase : IClassFixture<HomeControllerTestFixture>
        {
            private readonly HomeControllerTestBuilder _builder;

            public GivenPricesInTheDatabase(HomeControllerTestFixture fixture) 
                => _builder = fixture.Builder;

            [Fact]
            public async Task WhenGettingAllPrices_ChartDataHasBeenFilled()
            {
                var items = new List<ItemPrice>
                {
                    new ItemPrice
                        {Timestamp = new DateTime(2018, 9, 19), PartitionKey = "007", Price = "£3.00"},
                    new ItemPrice
                        {Timestamp = new DateTime(2018, 9, 20), PartitionKey = "007", Price = "£4.00"},
                };

                var repository = new ItemPriceRepositoryWithSpecifiedPrices(items);
                var homeController = _builder
                    .WithItemPriceRepository(repository)
                    .BuildController();
                
                var viewResult = await homeController.AllPrices() as ViewResult;
                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);

                var viewModel = viewResult.Model as ChartData;
                Assert.NotNull(viewModel);
                viewModel.Should().BeEquivalentTo(CreateChartData(items));
            }

            [Fact]
            public async Task WhenPriceIsNull_ThenChartDataHasNoValueForThatDay()
            {
                var items = new List<ItemPrice>
                {
                    new ItemPrice
                    {
                        Price = null,
                        PartitionKey = "007",
                        Timestamp = new DateTime(2018, 9, 19)
                    },
                    new ItemPrice
                    {
                        Price = "£4.00",
                        PartitionKey = "007",
                        Timestamp = new DateTime(2018, 9, 20)
                    }
                };

                var repository = new ItemPriceRepositoryWithSpecifiedPrices(items);
                var homeController = _builder
                    .WithItemPriceRepository(repository)
                    .BuildController();

                var viewResult = await homeController.AllPrices() as ViewResult;
                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);

                var viewModel = viewResult.Model as ChartData;
                Assert.NotNull(viewModel);
                viewModel.Should().BeEquivalentTo(CreateChartData(items));
            }

            private static ChartData CreateChartData(IReadOnlyCollection<ItemPrice> prices) 
                => ChartData.FromItemPrices(prices);
        }
    }
}