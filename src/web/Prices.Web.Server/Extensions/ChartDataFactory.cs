using System;
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
            var pricesDates = prices
                .Select(p => p.PriceDate.Date)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var chartData = new ChartData
            {
                Labels = pricesDates.Select(p => p.ToString("yyyy-MM-dd")).ToList()
            };

            foreach (var groupedPrice in prices.GroupBy(price => price.PartitionKey))
                chartData.DataSets.Add(new ChatDataSets
                {
                    Label = groupedPrice.Key,
                    Data = BuildData(groupedPrice.OrderBy(price => price.PriceDate.Date).ToList(), pricesDates)
                });

            return chartData;
        }

        private static List<string> BuildData(IReadOnlyCollection<ItemPriceEntity> itemPrices, 
            IEnumerable<DateTime> dateTimes) => dateTimes
            .Select(pricesDate => itemPrices.FirstOrDefault(x => x.PriceDate.Date == pricesDate.Date))
            .Select(priceEntity => priceEntity == null ? string.Empty : GetPriceFromRecord(priceEntity))
            .ToList();

        private static string GetPriceFromRecord(ItemPriceEntity otherThing) 
            => string.IsNullOrWhiteSpace(otherThing.Price) ? "" : otherThing.Price.Remove(0, 1);
    }
}