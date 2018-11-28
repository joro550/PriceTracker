using System;
using Microsoft.Extensions.Logging;

namespace PriceFinder.Tests.Stubs
{
    public class StubLogger : ILogger, IDisposable
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel) 
            => true;

        public IDisposable BeginScope<TState>(TState state) 
            => this;

        public void Dispose()
        {
        }
    }
}