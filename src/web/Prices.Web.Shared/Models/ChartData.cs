using System.Collections.Generic;

namespace Prices.Web.Shared.Models
{
    public class ChartData
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<ChatDataSets> DataSets { get; set; } = new List<ChatDataSets>();
    }
}