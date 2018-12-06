using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace PriceFinder.Models
{
    public class ItemPrice : TableEntity
    {
        public string Price { get; set; }
        public DateTime PriceDate { get; set; }

        public static ItemPrice FromQueueItem(QueueItem queueItem) => new ItemPrice
        {
            PartitionKey = queueItem.Id,
            RowKey = $"{Guid.NewGuid():N}",
            Price = "",
            PriceDate = DateTime.UtcNow
        };
        
        public static ItemPrice FromQueueItem(QueueItem queueItem, string price) => new ItemPrice
        {
            PartitionKey = queueItem.Id,
            RowKey = $"{Guid.NewGuid():N}",
            Price = price,
            PriceDate = DateTime.UtcNow
        };

    }
}