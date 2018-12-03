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
    public class GetArgosPriceFunction
    {
        public static HttpClient Client { get; set; } = new HttpClient();
        private static readonly List<string> HtmlClassesToCheck = new List<string> { "price" };

        [FunctionName("GetArgosPrice")]
        public static async Task Run(
            [QueueTrigger("argos-item-queue", Connection = "QueueConnectionString")]CloudQueueMessage message,
            [Table("prices", Connection = "TableConnectionString")] CloudTable prices)
        {
            var queueItem = message.GetMessageAs<QueueItem>();
            var itemResult = await Client.GetAsync($"https://www.argos.co.uk/product/{queueItem.Id}");

            var htmlParser = new HtmlParser();
            var pageContent = await itemResult.Content.ReadAsStringAsync();
            var document = await htmlParser.ParseAsync(pageContent);

            IElement dealPriceElement = null;

            foreach (var htmlClass in HtmlClassesToCheck)
            {
                dealPriceElement = document.QuerySelectorAll($".{htmlClass}").FirstOrDefault();
                if (dealPriceElement != null)
                    break;
            }

            var attributeValue = $"£{dealPriceElement?.GetAttribute("content")}";
            await prices.ExecuteAsync(TableOperation.Insert(new ItemPrice
            {
                PartitionKey = queueItem.Id,
                RowKey = $"{Guid.NewGuid():N}",
                Price = attributeValue,
                PriceDate = DateTime.UtcNow
            }));
        }
    }
}