using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace PriceFinder.Models
{
    public class ItemPrice : TableEntity
    {
        public string Price { get; set; }
        public DateTime PriceDate { get; set; }
    }
}