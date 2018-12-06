using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ChartJs.Blazor.ChartJS.LineChart;
using Prices.Web.Client.Pages.ItemPrices;
using Prices.Web.Client.Tests.Fakes;
using Prices.Web.Shared.Models;
using Xunit;

namespace Prices.Web.Client.Tests.Pages.ItemPrices
{
    public class PriceChartComponentTests
    {
        private readonly PriceChartComponentBuilder _builder;

        public PriceChartComponentTests() 
            => _builder = new PriceChartComponentBuilder();

        [Fact]
        public async Task WhenInitializing_ComponentMakesCallToGetPrices()
        {
            var messageHandler = FakeHttpMessageHandler.WithNoCotentResult();
            var component = _builder
                .WithMessageHandler(messageHandler)
                .Build();
            await component.InitAsync();
            Assert.Contains("/api/prices/ChartData", messageHandler.GetRequests());
        }

        [Fact]
        public async Task WhenNotFoundIsReturned_ThenChartIsNotLoaded()
        {
            var component = _builder.Build();
            await component.InitAsync();
            Assert.NotNull(component.GetChartConfig());
        }

        [Fact]
        public async Task WhenChartDataIsValid_ThenLabelsAreLoaded()
        {
            var chartData = new ChartData
            {
                Labels = new List<string> {"1", "2", "3"}
            };
            
            var component = _builder
                .WithMessageHandler(FakeHttpMessageHandler.WithResult(chartData))
                .Build();
            await component.InitAsync();
            
            var config = component.GetChartConfig();
            Assert.Contains("1", config.Data.Labels);
            Assert.Contains("2", config.Data.Labels);
            Assert.Contains("3", config.Data.Labels);
        }

        [Fact]
        public async Task WhenChartDataIsValid_ThenDataSetHasBeenLoaded()
        {
            var chartData = new ChartData {Labels = new List<string> {"1", "2", "3"}};
            chartData.DataSets.Add(new ChatDataSets {Label = "1", Data = new List<string> {"5", "6", "7"}});
            
            var component = _builder
                .WithMessageHandler(FakeHttpMessageHandler.WithResult(chartData))
                .Build();
            await component.InitAsync();
            
            var config = component.GetChartConfig();
            Assert.Equal(new List<object> {"5", "6", "7"}, config.Data.Datasets.First().Data);
        }
    }

    public class PriceChartComponentWrapper : PriceChartComponent
    {
        public PriceChartComponentWrapper(HttpClient client) 
            => Client = client;
        
        public async Task InitAsync() 
            => await OnInitAsync();

        public LineChartConfig GetChartConfig() 
            => ChartConfig;
    }
    
    public class PriceChartComponentBuilder
    {
        private HttpClient _client;

        public PriceChartComponentBuilder WithMessageHandler(HttpMessageHandler messageHandler)
        {
            _client = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            return this;
        }

        public PriceChartComponentWrapper Build()
        {
            var client = _client ?? new HttpClient(FakeHttpMessageHandler.WithNotFoundResult())
            {
                BaseAddress = new Uri("http://localhost/")
            };

            return new PriceChartComponentWrapper(client);
        }
    }
}