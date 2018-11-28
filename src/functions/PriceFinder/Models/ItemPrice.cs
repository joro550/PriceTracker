using Microsoft.WindowsAzure.Storage.Table;

namespace PriceFinder.Models
{
    public class ItemPrice : TableEntity
    {
        public string Price { get; set; }
    }
}