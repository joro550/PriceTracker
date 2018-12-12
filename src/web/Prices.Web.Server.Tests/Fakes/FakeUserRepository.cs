using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prices.Web.Server.Data;
using Prices.Web.Server.Data.Entities;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeUserRepository : InMemoryRepository<UserEntity>, IUserRepository
    {
        public static UserEntity NormalUser 
            => new UserEntity {Id = "1", Username = "username", Password = "password"};

        private static List<UserEntity> DefaultUsers =>  new List<UserEntity>
        {
            NormalUser
        };
        
        private FakeUserRepository(List<UserEntity> items) 
            : base(items)
        {
        }

        public static FakeUserRepository WithNoRecords() 
            => new FakeUserRepository(new List<UserEntity>());

        public static FakeUserRepository WithDefaultUsers() 
            => new FakeUserRepository(DefaultUsers);

        public Task<UserEntity> GetByUsername(string userUserName) 
            => Task.FromResult(base.Items.FirstOrDefault(entity => entity.Username == userUserName));
    }
}