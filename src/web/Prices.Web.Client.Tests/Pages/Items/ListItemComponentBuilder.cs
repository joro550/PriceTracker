using System;
using System.Net.Http;
using Prices.Web.Client.Tests.Fakes;

namespace Prices.Web.Client.Tests.Pages.Items
{
    public class ListItemComponentBuilder
    {
        private HttpClient _client;

        public ListItemComponentBuilder WithMessageHandler(HttpMessageHandler messageHandler)
        {
            _client = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            return this;
        }

        public ListItemComponentWrapper Build()
        {
            var client = _client ?? new HttpClient(FakeHttpMessageHandler.WithNotFoundResult())
            {
                BaseAddress = new Uri("http://localhost/")
            };

            return new ListItemComponentWrapper(client);
        }
    }
}