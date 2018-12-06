using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Prices.Web.Client.Tests.Fakes
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly List<Uri> _requests = new List<Uri>();
        private readonly HttpResponseMessage _responseMessage;

        private FakeHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            _requests.Add(request.RequestUri);
            return Task.FromResult(_responseMessage);
        }

        public static FakeHttpMessageHandler WithNotFoundResult() => new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        });
        
        public static FakeHttpMessageHandler WithNoCotentResult() => new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        });


        public static FakeHttpMessageHandler WithResult<T>(T result) => new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(Json.Serialize(result))
        });

        public IEnumerable<string> GetRequests()
        {
            return _requests.Select(r => r.AbsolutePath).ToList();
        }
    }

    public class FakeHttpMessageHandlerTests
    {
    }
}