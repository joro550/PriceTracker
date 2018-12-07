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
                .Select(p => p.PriceDate)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var chartData = new ChartData
            {
                Labels = pricesDates.Select(p => p.ToString("yyyy-MM-dd")).ToList()
            };

            foreach (var groupedPrice in prices.GroupBy(price => price.PartitionKey))
            {
                var dataSet = new ChatDataSets {Label = groupedPrice.Key};

                var priceEntities = groupedPrice.OrderBy(price => price.PriceDate).ToList();
                foreach (var pricesDate in pricesDates)
                {
                    var priceEntity = priceEntities.FirstOrDefault(x => x.PriceDate.Date == pricesDate.Date);
                    dataSet.Data.Add(priceEntity == null ? string.Empty : GetPriceFromRecord(priceEntity));
                }

                chartData.DataSets.Add(dataSet);
            }

            return chartData;
        }

        private static string GetPriceFromRecord(ItemPriceEntity otherThing) 
            => string.IsNullOrWhiteSpace(otherThing.Price) ? "" : otherThing.Price.Remove(0, 1);
    }
}