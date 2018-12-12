namespace Prices.Web.Server.Tests.Identity.UserStore
{
    public class UserStoreFixture
    {
        public UserStoreBuilder Builder { get; }

        public UserStoreFixture()
        {
            Builder = new UserStoreBuilder();
        }
    }
}