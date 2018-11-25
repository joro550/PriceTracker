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

        public FileLoaderMessageHandler(string itemType)
        {
            _itemType = itemType;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var name = assembly.GetManifestResourceNames()
                .First(resource => resource.EndsWith($"{_itemType}.html"));

            using (var stream = assembly.GetManifestResourceStream(name))
            using (var streamReader = new StreamReader(stream))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(streamReader.ReadToEnd())
                });
            }
        }
    }
}