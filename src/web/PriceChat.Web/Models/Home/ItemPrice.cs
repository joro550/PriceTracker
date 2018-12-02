using System;

namespace PriceChat.Web.Models.Home
{
    public class ItemPrice
    {
        public string PartitionKey { get; set; }
        public string Price { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime PriceDate { get; set; }
    }
}