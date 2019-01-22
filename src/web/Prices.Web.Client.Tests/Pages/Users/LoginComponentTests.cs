using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Prices.Web.Client.Tests.Fakes;
using Prices.Web.Shared.Models.Users;
using Xunit;

namespace Prices.Web.Client.Tests.Pages.Users
{
    public class LoginComponentTests
    {
        [Fact]
        public async Task WhenBadRequestIsReturned_ThenNoTokenIsGivenToTheState()
        {
            var component = new LoginComponentBuilder()
                .WithMessageHandler(FakeHttpMessageHandler.WithBadRequestResponse())
                .Build();
            await component.Login();
            Assert.Empty(component.GetUserToken());
            Assert.True(component.ShowLoginErrors);
        }
        
        [Theory, AutoData]
        public async Task WhenResponseIsGiven_ThenTokenIsWrittenIntoState(string expectedToken)
        {
            var tokenResult = new TokenResult {Token = expectedToken};
            var fakeUriHelper = new FakeUriHelper();
            var component = new LoginComponentBuilder()
                .WithMessageHandler(FakeHttpMessageHandler.WithResult(tokenResult))
                .WithUriHelper(fakeUriHelper)
                .Build();
            
            await component.Login();
            Assert.Equal(expectedToken, component.GetUserToken());
            Assert.False(component.ShowLoginErrors);
            Assert.Equal("/", fakeUriHelper.Uri);
        }
    }
}