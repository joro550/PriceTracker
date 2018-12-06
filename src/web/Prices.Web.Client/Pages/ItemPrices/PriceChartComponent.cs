using System.Collections.Generic;
using System.Threading.Tasks;
using ChartJs.Blazor.ChartJS.Common;
using ChartJs.Blazor.ChartJS.Common.Legends;
using ChartJs.Blazor.ChartJS.LineChart;
using ChartJs.Blazor.Charts;
using Microsoft.AspNetCore.Blazor.Components;

namespace Prices.Web.Client.Pages.ItemPrices
{
    public class PriceChartComponent : BlazorComponent
    {
        protected LineChartConfig pieChartConfig;
        protected ChartJsLineChart lineChartJs;

        protected override Task OnInitAsync()
        {
            pieChartConfig = pieChartConfig ?? new LineChartConfig
            {
                CanvasId = "myFirstLineChart",
                Options = new LineChartOptions
                {
                    Text = "Sample chart from Blazor",
                    Display = true,
                    Responsive = true,
                    Title = new OptionsTitle {Display = true, Text = "Line Chart"},
                    Legend = new Legend
                    {
                        Position = LegendPosition.BOTTOM.ToString(),
                        Labels = new Labels
                        {
                            UsePointStyle = true
                        }
                    },
                    Tooltips = new Tooltips
                    {
                        Mode = Mode.nearest,
                        Intersect = false
                    },
                    Hover = new LineChartOptionsHover
                    {
                        Intersect = true,
                        Mode = Mode.nearest
                    }
                },
                Data = new LineChartData
                {
                    Labels = new List<string> {"Red", "Blue", "Yellow", "Green", "Purple", "Orange"},
                    Datasets = new List<LineChartDataset>
                    {
                        new LineChartDataset
                        {
                            BackgroundColor = "#ff6384",
                            BorderColor = "#ff6384",
                            Label = "# of Votes from blazor",
                            Data = new List<object> {4, 6, 2, 7, 9, 1},
                            Fill = false,
                            BorderWidth = 2,
                            PointRadius = 3,
                            PointBorderWidth = 1
                        }
                    }
                }
            };
            return base.OnInitAsync();
        }
    }
}