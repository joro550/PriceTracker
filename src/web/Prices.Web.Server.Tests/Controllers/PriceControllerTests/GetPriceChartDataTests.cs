using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Prices.Web.Server.Handlers.Data.Entities;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models;
using Xunit;

namespace Prices.Web.Server.Tests.Controllers.PriceControllerTests
{
    public class GetPriceChartDataTests: IClassFixture<WebApplicationFixture>
    {
        private readonly WebApplicationBuilder _fixture;

        public GetPriceChartDataTests(WebApplicationFixture fixture) 
            => _fixture = fixture.ApplicationBuilder;

        [Fact]
        public async Task WhenNoPricesExist_ThenNoContentResultIsReturned()
        {
            var webApplication = _fixture
                .WithItemPriceRepository(FakeItemPriceRepository.WithNoPrices())
                .Build();
            var response = await webApplication.GetAsync("/api/prices/ChartData");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task WhenPricesExist_ThenChartDataLabelsHaveBeenLoadedSuccessfully()
        {
            var prices = new List<ItemPriceEntity>
            {
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,1)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,2)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,3)}
            };
            
            var webApplication = _fixture
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            var response = await webApplication.GetJsonAsync<ChartData>("/api/prices/ChartData");
            
            Assert.Contains("2018-01-01", response.Labels);
            Assert.Contains("2018-01-02", response.Labels);
            Assert.Contains("2018-01-03", response.Labels);
        }

        [Fact]
        public async Task WhenPricesExist_ThenChartDataLabelsHaveDistinctData()
        {
            var prices = new List<ItemPriceEntity>
            {
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,1)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,1)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,3)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,2)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,2)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,3)}
            };

            var webApplication = _fixture
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            
            var response = await webApplication.GetJsonAsync<ChartData>("/api/prices/ChartData");
            Assert.Single(response.Labels.Where(l => l == "2018-01-01"));
            Assert.Single(response.Labels.Where(l => l == "2018-01-02"));
            Assert.Single(response.Labels.Where(l => l == "2018-01-03"));
        }

        [Fact]
        public async Task WhenPricesExistWithTimeInformation_ThenChartDataLabelsHaveDistinctData()
        {
            var prices = new List<ItemPriceEntity>
            {
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,1,12,12,12)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,1,13,13,13)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,3)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,2)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,2)},
                new ItemPriceEntity { PriceDate = new DateTime(2018,1,3)}
            };
            
            var webApplication = _fixture
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            
            var response = await webApplication.GetJsonAsync<ChartData>("/api/prices/ChartData");
            Assert.Single(response.Labels.Where(l => l == "2018-01-01"));
            Assert.Single(response.Labels.Where(l => l == "2018-01-02"));
            Assert.Single(response.Labels.Where(l => l == "2018-01-03"));
        }

        [Fact]
        public async Task WhenPricesExist_ThenChartDataPricesAreLoaded()
        {
            var prices = new List<ItemPriceEntity>
            {
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,1), Price = "£1.00"},
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,2), Price = "£5.00"},
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,3), Price = "£6.00"},
            };
            
            var webApplication = _fixture
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            
            var response = await webApplication.GetJsonAsync<ChartData>("/api/prices/ChartData");
            Assert.Contains("1.00", response.DataSets.SelectMany(ds => ds.Data));
            Assert.Contains("5.00", response.DataSets.SelectMany(ds => ds.Data));
            Assert.Contains("6.00", response.DataSets.SelectMany(ds => ds.Data));
        }

        [Fact]
        public async Task WhenPricesExist_ThenChartDataPricesAreLoadedInDateOrder()
        {
            var prices = new List<ItemPriceEntity>
            {
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,3), Price = "£1.00"},
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,1), Price = "£5.00"},
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,5), Price = "£6.00"},
            };
            
            var webApplication = _fixture
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            
            var response = await webApplication.GetJsonAsync<ChartData>("/api/prices/ChartData");
            Assert.Contains("5.00", response.DataSets[0].Data[0]);
            Assert.Contains("1.00", response.DataSets[0].Data[1]);
            Assert.Contains("6.00", response.DataSets[0].Data[2]);
        }

        [Fact]
        public async Task WhenMultiplePricesExistAndOneDoesNotHavePriceOnDate_ThenChartDataPricesWithBlankAsMissingPrice()
        {
            var prices = new List<ItemPriceEntity>
            {
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018, 1, 3, 10, 10, 10), Price = "£1.00"},
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018, 1, 1), Price = "£5.00"},
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018, 1, 5, 12, 12, 12), Price = "£6.00"},

                new ItemPriceEntity {PartitionKey = "2", PriceDate = new DateTime(2018, 1, 3, 11, 11, 11), Price = "£1.00"},
                new ItemPriceEntity {PartitionKey = "2", PriceDate = new DateTime(2018, 1, 5), Price = "£6.00"},
            };
            
            var webApplication = _fixture
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            
            var response = await webApplication.GetJsonAsync<ChartData>("/api/prices/ChartData");
            Assert.Contains("5.00", response.DataSets[0].Data[0]);
            Assert.Contains("1.00", response.DataSets[0].Data[1]);
            Assert.Contains("6.00", response.DataSets[0].Data[2]);
            
            Assert.Contains("", response.DataSets[1].Data[0]);
            Assert.Contains("1.00", response.DataSets[1].Data[1]);
            Assert.Contains("6.00", response.DataSets[1].Data[2]);
        }
    }
}