using System;
using Microsoft.AspNetCore.Blazor.Services;

namespace Prices.Web.Client.Tests.Fakes
{
    public class FakeUriHelper : IUriHelper
    {
        public string Uri { get; private set; }
        public event EventHandler<string> OnLocationChanged;

        public FakeUriHelper() 
            => Uri = string.Empty;

        public string GetAbsoluteUri()
        {
            throw new NotImplementedException();
        }

        public Uri ToAbsoluteUri(string href)
        {
            throw new NotImplementedException();
        }

        public string GetBaseUri()
        {
            throw new NotImplementedException();
        }

        public string ToBaseRelativePath(string baseUri, string locationAbsolute)
        {
            throw new NotImplementedException();
        }

        
        public void NavigateTo(string uri) => Uri = uri;
    }
}