using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Data;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models;
using Xunit;

namespace Prices.Web.Server.Tests.Controllers.PriceControllerTests
{
    public class GetPriceChartDataTests
    {
        private readonly PriceControllerBuilder _priceControllerBuilder;

        public GetPriceChartDataTests() 
            => _priceControllerBuilder = new PriceControllerBuilder();

        [Fact]
        public async Task HasAllItemChartDataMethod()
        {
            var prices = new List<ItemPriceEntity> {new ItemPriceEntity()};
            var controller = _priceControllerBuilder
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            Assert.IsType<OkObjectResult>(await controller.PriceChartData());
        }

        [Fact]
        public async Task WhenNoPricesExist_ThenNoContentResultIsReturned()
        {
            var controller = _priceControllerBuilder
                .WithItemPriceRepository(FakeItemPriceRepository.WithNoPrices())
                .Build();
            Assert.IsType<NoContentResult>(await controller.PriceChartData());
        }

        [Fact]
        public async Task WhenPricesExist_ThenChartDataIsReturned()
        {
            var prices = new List<ItemPriceEntity> {new ItemPriceEntity()};
            
            var controller = _priceControllerBuilder
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            var result = Assert.IsType<OkObjectResult>(await controller.PriceChartData());
            Assert.IsType<ChartData>(result.Value);
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
            
            var controller = _priceControllerBuilder
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            var result = Assert.IsType<OkObjectResult>(await controller.PriceChartData());
            var model = Assert.IsType<ChartData>(result.Value);
            
            Assert.Contains("2018-01-01", model.Labels);
            Assert.Contains("2018-01-02", model.Labels);
            Assert.Contains("2018-01-03", model.Labels);
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
            
            var controller = _priceControllerBuilder
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            var result = Assert.IsType<OkObjectResult>(await controller.PriceChartData());
            var model = Assert.IsType<ChartData>(result.Value);
            Assert.Single(model.Labels.Where(l => l == "2018-01-01"));
            Assert.Single(model.Labels.Where(l => l == "2018-01-02"));
            Assert.Single(model.Labels.Where(l => l == "2018-01-03"));
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
            
            var controller = _priceControllerBuilder
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            var result = Assert.IsType<OkObjectResult>(await controller.PriceChartData());
            var model = Assert.IsType<ChartData>(result.Value);
            Assert.Contains("1.00", model.DataSets.SelectMany(ds => ds.Data));
            Assert.Contains("5.00", model.DataSets.SelectMany(ds => ds.Data));
            Assert.Contains("6.00", model.DataSets.SelectMany(ds => ds.Data));
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
            
            var controller = _priceControllerBuilder
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            var result = Assert.IsType<OkObjectResult>(await controller.PriceChartData());
            var model = Assert.IsType<ChartData>(result.Value);
            Assert.Contains("5.00", model.DataSets[0].Data[0]);
            Assert.Contains("1.00", model.DataSets[0].Data[1]);
            Assert.Contains("6.00", model.DataSets[0].Data[2]);
        }
        
        [Fact]
        public async Task WhenMultiplePricesExistAndOneDoesNotHavePriceOnDate_ThenChartDataPricesWithBlankAsMissingPrice()
        {
            var prices = new List<ItemPriceEntity>
            {
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,3), Price = "£1.00"},
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,1), Price = "£5.00"},
                new ItemPriceEntity {PartitionKey = "1", PriceDate = new DateTime(2018,1,5), Price = "£6.00"},

                new ItemPriceEntity {PartitionKey = "2", PriceDate = new DateTime(2018,1,3), Price = "£1.00"},
                new ItemPriceEntity {PartitionKey = "2", PriceDate = new DateTime(2018,1,5), Price = "£6.00"},

            };
            
            var controller = _priceControllerBuilder
                .WithItemPriceRepository(FakeItemPriceRepository.WithPrices(prices))
                .Build();
            var result = Assert.IsType<OkObjectResult>(await controller.PriceChartData());
            var model = Assert.IsType<ChartData>(result.Value);
            Assert.Contains("5.00", model.DataSets[0].Data[0]);
            Assert.Contains("1.00", model.DataSets[0].Data[1]);
            Assert.Contains("6.00", model.DataSets[0].Data[2]);
            
            Assert.Contains("", model.DataSets[1].Data[0]);
            Assert.Contains("1.00", model.DataSets[1].Data[1]);
            Assert.Contains("6.00", model.DataSets[1].Data[2]);
        }
    }
}