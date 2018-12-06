using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using PriceFinder.Extensions;
using PriceFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            if (dealPriceElement == null)
            {
                await prices.InsertEntity(ItemPrice.FromQueueItem(queueItem));
            }
            else
            {
                var itemPrice = IsPrice(dealPriceElement)
                    ? ItemPrice.FromQueueItem(queueItem, dealPriceElement?.InnerHtml.Replace(",", string.Empty))
                    : ItemPrice.FromQueueItem(queueItem);

                await prices.InsertEntity(itemPrice);
            }
        }

        private static bool IsPrice(IElement dealPriceElement) 
            => Regex.IsMatch(dealPriceElement.InnerHtml, "(£)?\\d{1,3}(?:[.,]\\d{3})*(?:[.,]\\d{2})?");

        private static IElement GetPriceElement(IParentNode document)
        {
            foreach (var htmlId in HtmlIdsToCheck)
            {
                var idElement = document.QuerySelectorAll($"#{htmlId}").FirstOrDefault();
                if (idElement != null)
                    return idElement;
            }

            foreach (var htmlClass in HtmlClassesToCheck)
            {
                var classElement = document.QuerySelectorAll($".{htmlClass}").FirstOrDefault();
                if (classElement != null)
                    return classElement;
            }

            return null;
        }
    }
}