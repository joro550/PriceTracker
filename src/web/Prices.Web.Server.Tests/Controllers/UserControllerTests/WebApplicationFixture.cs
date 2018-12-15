namespace Prices.Web.Server.Tests.Controllers.UserControllerTests
{
    public class WebApplicationFixture
    {
        protected internal readonly WebApplicationBuilder ApplicationBuilder;

        public WebApplicationFixture() 
            => ApplicationBuilder = new WebApplicationBuilder();
    }
}