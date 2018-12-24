using System.Threading.Tasks;
using Prices.Web.Client.Tests.Fakes;
using Xunit;

namespace Prices.Web.Client.Tests.Pages.Items.AddItems
{
    public class InitTests
    {
        [Fact]
        public void WhenStateHasNoUserToken_UserIsNavigatedToHomePage()
        {
            var uriHelper = new FakeUriHelper();
            var component = new AddItemComponentBuilder()
                .WithUriHelper(uriHelper)
                .Build();
            
            component.Init();
            Assert.Equal("/", uriHelper.Uri);
        }
        
        [Fact]
        public async Task WhenStateHasNoUserToken_InitializationDoesNotNavigateAway()
        {
            var uriHelper = new FakeUriHelper();
            var component = new AddItemComponentBuilder()
                .WithUriHelper(uriHelper)
                .Build();

            await component.AddUserToken("userToken");
            component.Init();
            Assert.Equal(string.Empty, uriHelper.Uri);
        }
    }
}