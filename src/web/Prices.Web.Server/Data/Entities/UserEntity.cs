using Microsoft.WindowsAzure.Storage.Table;

namespace Prices.Web.Server.Data.Entities
{
    public class UserEntity : TableEntity
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}