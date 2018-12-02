using System;
using System.Collections.Generic;

namespace PriceChat.Web.Models.Home
{
    public class ItemModel
    {
        public string Id { get; set; }
        public ChartData Prices { get; set; } = new ChartData();
    }


    public class ItemPrice
    {
        public string PartitionKey { get; set; }
        public string Price { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime PriceDate { get; set; }
    }
}