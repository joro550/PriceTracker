using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using PriceFinder.Models;

namespace PriceFinder
{
    public static class GetItemsFunctions
    {
        [FunctionName("GetItemsOntoQueue")]
        public static async Task Run([TimerTrigger("0 30 9 * * *")]TimerInfo myTimer,
            [Table("items", Connection = "TableConnectionString")] CloudTable items, 
            [Queue("item-queue", Connection= "QueueConnectionString")]CloudQueue queue,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var query = new TableQuery<Item>();
            var segment = await items.ExecuteQuerySegmentedAsync(query, null);

            foreach (var item in segment.Results)
            {
                var queueItem = new QueueItem {Id = item.Id};
                await queue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(queueItem)));
            }
        }
    }
}
