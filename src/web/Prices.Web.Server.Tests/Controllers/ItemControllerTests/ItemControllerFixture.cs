namespace Prices.Web.Server.Tests.Controllers.ItemControllerTests
{
    public class ItemControllerFixture
    {
        public ItemControllerBuilder Builder { get; }

        public ItemControllerFixture() 
            => Builder = new ItemControllerBuilder();
    }
}