using System.Collections.Generic;
using System.Linq;

namespace PriceChat.Web.Models
{
    public class ChartData
    {
        public List<string> Labels { get; private set; } = new List<string>();
        public List<ChatDataSets> DataSets { get; } = new List<ChatDataSets>();

        public static ChartData FromItemPrices(IReadOnlyCollection<Data.ItemPrice> prices)
        {
            var chartData = new ChartData
            {
                Labels = prices.Select(p => p.Timestamp.ToString("yyyy-M-d")).Distinct().ToList()
            };

            foreach (var groupedPrice in prices.GroupBy(price => price.PartitionKey))
            {
                var dataSet = new ChatDataSets{Label = groupedPrice.Key};
                foreach (var itemPrice in groupedPrice.OrderBy(price => price.Timestamp))
                {
                    dataSet.Data.Add(itemPrice.Price.Remove(0, 1));
                }

                chartData.DataSets.Add(dataSet);
            }
            return chartData;
        }
    }
}