using System.Linq;
using PriceChat.Web.Data;
using System.Collections.Generic;

namespace PriceChat.Web.Models
{
    public class ChartData
    {
        public List<string> Labels { get; private set; } = new List<string>();
        public List<ChatDataSets> DataSets { get; } = new List<ChatDataSets>();

        public static ChartData FromItemPrices(IReadOnlyCollection<ItemPriceEntity> prices)
        {
            var chartData = new ChartData
            {
                Labels = prices.Select(p => p.PriceDate.ToString("yyyy-M-d")).Distinct().OrderBy(x => x).ToList()
            };

            foreach (var groupedPrice in prices.GroupBy(price => price.PartitionKey))
            {
                var dataSet = new ChatDataSets{Label = groupedPrice.Key};
                foreach (var itemPrice in groupedPrice.OrderBy(price => price.PriceDate))
                    dataSet.Data.Add(string.IsNullOrWhiteSpace(itemPrice.Price) ? "" : itemPrice.Price.Remove(0, 1));

                chartData.DataSets.Add(dataSet);
            }
            return chartData;
        }
    }
}