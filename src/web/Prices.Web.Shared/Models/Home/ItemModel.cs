namespace Prices.Web.Shared.Models.Home
{
    public class ItemModel
    {
        public string Id { get; set; }
        public ChartData Prices { get; set; } = new ChartData();
    }
}