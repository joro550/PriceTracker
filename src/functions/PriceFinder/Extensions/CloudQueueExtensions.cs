using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace PriceFinder.Extensions
{
    public static class CloudQueueExtensions
    {
        public static T GetMessageAs<T>(this CloudQueueMessage message) 
            => JsonConvert.DeserializeObject<T>(message.AsString);
    }
}