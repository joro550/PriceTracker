namespace PriceChat.Web.Models.Home
{
    public class ItemModel
    {
        public string Id { get; set; }
        public ChartData Prices { get; set; } = new ChartData();
    }
}