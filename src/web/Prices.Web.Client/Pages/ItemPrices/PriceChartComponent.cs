using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChartJs.Blazor.ChartJS.Common;
using ChartJs.Blazor.ChartJS.Common.Legends;
using ChartJs.Blazor.ChartJS.LineChart;
using ChartJs.Blazor.Charts;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Prices.Web.Shared.Models;

namespace Prices.Web.Client.Pages.ItemPrices
{
    public class PriceChartComponent : BlazorComponent
    {
        private readonly Random _random = new Random();

        protected ChartJsLineChart LineChartJs;
        protected LineChartConfig ChartConfig { set; get; } = new LineChartConfig();
        
        [Inject] protected HttpClient Client { get; set; }
        [Inject] protected ILogger<PriceChartComponent> Logger { get; set; }

        protected override async Task OnInitAsync()
        {
            ChartConfig = new LineChartConfig();

            var chartData = await Client.GetAsync("/api/prices/ChartData");
            ChartConfig = chartData.IsSuccessStatusCode 
                ? await BuildChartConfig(chartData) 
                : new LineChartConfig();
            
//            LineChartJs.Reload();
        }

        protected override Task OnAfterRenderAsync()
        {
            LineChartJs.Reload();

            return base.OnAfterRenderAsync();
        }

        private async Task<LineChartConfig> BuildChartConfig(HttpResponseMessage responseMessage)
        {
            var contentString = await responseMessage.Content.ReadAsStringAsync();
            var chartData = Json.Deserialize<ChartData>(contentString);

            var buildChartConfig = new LineChartConfig
            {
                CanvasId = "myFirstLineChart",
                Options = new LineChartOptions
                {
                    Text = "Item Prices",
                    Display = true,
                    Responsive = true,
                    Title = new OptionsTitle {Display = true, Text = "Item Prices"},
                    Legend = new Legend
                    {
                        Position = LegendPosition.TOP.ToString(),
                        Labels = new Labels
                        {
                            UsePointStyle = true,
                        }
                    },
                    Hover = new LineChartOptionsHover()
                    {
                        Intersect = true,
                        Mode = Mode.nearest
                    },
                    Tooltips = new Tooltips
                    {
                        Mode = Mode.nearest,
                        Intersect = true
                    },
                },
                Data = new LineChartData
                {
                    Labels = chartData.Labels,
                    Datasets = new List<LineChartDataset>()
                }
            };

            foreach (var dataSet in chartData.DataSets)
            {
                var chartDataSet = new LineChartDataset
                {
                    Label = dataSet.Label, 
                    BackgroundColor = GetRandomColor(),
                    BorderColor = GetRandomColor(), 
                    BorderWidth = 2,
                    PointRadius = 3,
                    PointBorderWidth = 1,
                    Data = new List<object>()
                };
                
                foreach (var datum in dataSet.Data)
                    chartDataSet.Data.Add(datum);

                buildChartConfig.Data.Datasets.Add(chartDataSet);
            }
            return  buildChartConfig;
        }

        private string GetRandomColor()
        {
            const string letters = "0123456789ABCDEF";
            var color = "#";
            for (var i = 0; i < 6; i++)
                color += letters[_random.Next(0, letters.Length - 1)];
            return color;
        }
    }
}