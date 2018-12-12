using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Prices.Web.Server.Data.Entities
{
    public class ItemPriceEntity : TableEntity
    {
        public string Price { get; set; }
        public DateTime PriceDate { get; set; }
    }
}