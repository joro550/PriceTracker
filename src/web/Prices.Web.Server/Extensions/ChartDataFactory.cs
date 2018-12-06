using System.Collections.Generic;
using System.Linq;
using Prices.Web.Server.Data;
using Prices.Web.Shared.Models;

namespace Prices.Web.Server.Extensions
{
    public static class ChartDataFactory
    {
        public static ChartData FromItemPrices(IReadOnlyCollection<ItemPriceEntity> prices)
        {
            var chartData = new ChartData
            {
                Labels = prices.Select(p => p.PriceDate.ToString("yyyy-MM-dd")).Distinct().OrderBy(x => x).ToList()
            };

            foreach (var groupedPrice in prices.GroupBy(price => price.PartitionKey))
            {
                var dataSet = new ChatDataSets {Label = groupedPrice.Key};
                foreach (var itemPrice in groupedPrice.OrderBy(price => price.PriceDate))
                    dataSet.Data.Add(string.IsNullOrWhiteSpace(itemPrice.Price) ? "" : itemPrice.Price.Remove(0, 1));

                chartData.DataSets.Add(dataSet);
            }

            return chartData;
        }
    }
}