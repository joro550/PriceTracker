using Microsoft.WindowsAzure.Storage.Table;

namespace Prices.Web.Server.Handlers.Data.Entities
{
    public class ItemEntity : TableEntity
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Retailer { get; set; }
    }
}