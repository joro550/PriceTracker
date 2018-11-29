using System;

namespace PriceChat.Web.Models
{
    public class ItemPrice
    {
        public string PartitionKey { get; set; }
        public string Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}