using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace PriceFinder.Tests.Extensions
{
    public static class CloudQueueExtensions
    {
        public static async Task<T> GetMessageAs<T>(this CloudQueue cloudQueue)
        {
            var message = await cloudQueue.GetMessageAsync();
            return JsonConvert.DeserializeObject<T>(message.AsString);
        }

        public static async Task<List<T>> GetMessagesAs<T>(this CloudQueue cloudQueue, int messageCount)
        {
            var message = await cloudQueue.GetMessagesAsync(messageCount);
            return message.Select(m => JsonConvert.DeserializeObject<T>(m.AsString)).ToList();
        }
    }
}