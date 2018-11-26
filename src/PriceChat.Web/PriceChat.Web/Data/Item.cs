using Microsoft.WindowsAzure.Storage.Table;

namespace PriceChat.Web.Data
{
    public class Item : TableEntity
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Retailer { get; set; }
    }
}