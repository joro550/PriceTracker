using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PriceFinder.Tests.Fakes
{
    public class FileLoaderMessageHandler : HttpMessageHandler
    {
        private readonly string _itemType;
        private readonly string _retailer;

        private readonly List<string> _requests = new List<string>();

        public FileLoaderMessageHandler(string itemType, string retailer)
        {
            _itemType = itemType;
            _retailer = retailer;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _requests.Add(request.RequestUri.ToString());

            var assembly = Assembly.GetExecutingAssembly();
            var name = assembly.GetManifestResourceNames()
                .First(resource => resource.EndsWith($"{_retailer}.{_itemType}.html"));

            using (var stream = assembly.GetManifestResourceStream(name))
            using (var streamReader = new StreamReader(stream))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(streamReader.ReadToEnd())
                });
            }
        }

        public IEnumerable<string> GetRequests() 
            => _requests;
    }
}