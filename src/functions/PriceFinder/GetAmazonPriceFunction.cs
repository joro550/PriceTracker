using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using PriceFinder.Extensions;
using PriceFinder.Models;

namespace PriceFinder
{
    public class GetAmazonPriceFunction
    {
        public static HttpClient Client { get; set; } = new HttpClient();
        private static readonly List<string> HtmlIdsToCheck = new List<string> { "priceblock_dealprice", "priceblock_ourprice" };
        private static readonly List<string> HtmlClassesToCheck = new List<string> {"offer-price", "a-color-price" };

        [FunctionName("GetPrice")]
        public static async Task Run(
            [QueueTrigger("amazon-item-queue", Connection = "QueueConnectionString")]CloudQueueMessage message,
            [Table("prices", Connection = "TableConnectionString")] CloudTable prices)
        {
            var queueItem = message.GetMessageAs<QueueItem>();
            var itemResult = await Client.GetAsync($"https://www.amazon.co.uk/dp/{queueItem.Id}");

            var htmlParser = new HtmlParser();
            var pageContent = await itemResult.Content.ReadAsStringAsync();
            var dealPriceElement = GetPriceElement(await htmlParser.ParseAsync(pageContent));

            await prices.ExecuteAsync(TableOperation.Insert(new ItemPrice
            {
                PartitionKey = queueItem.Id,
                RowKey = $"{Guid.NewGuid():N}",
                Price = dealPriceElement?.InnerHtml.Replace(",", string.Empty),
                PriceTime = DateTime.UtcNow
            }));
        }

        private static IElement GetPriceElement(IParentNode document)
        {
            IElement dealPriceElement;

            foreach (var htmlId in HtmlIdsToCheck)
            {
                dealPriceElement = document.QuerySelectorAll($"#{htmlId}").FirstOrDefault();
                if (dealPriceElement != null)
                    return dealPriceElement;
            }

            foreach (var htmlClass in HtmlClassesToCheck)
            {
                dealPriceElement = document.QuerySelectorAll($".{htmlClass}").FirstOrDefault();
                if (dealPriceElement != null)
                    return dealPriceElement; ;
            }

            return null;
        }
    }
}