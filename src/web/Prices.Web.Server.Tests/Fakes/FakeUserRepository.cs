using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeUserRepository : InMemoryRepository<UserEntity>, IUserRepository
    {
        public static TestUserEntity NormalUser
            => new TestUserEntity
            {
                Id = "1",
                Username = "username",
                Password = "bBSOWgFld0b6A9EL5VVK3yDFvdbiE4klAgdrDLcr6IUb+kNtDwwWCdnY44NKi92H44ikarLveXti0k3UCbVyAA==",
                PasswordSalt = "50.R/MOtq7wRiL9ZxmVREQMfQ==",
                OriginalPassword = "password"
            };

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
            => Task.FromResult(Items.FirstOrDefault(entity => entity.Username == userUserName));

        public Task<UserEntity> GetById(string userId)
            => Task.FromResult(Items.FirstOrDefault(entity => entity.Id == userId));
    }

    public class TestUserEntity : UserEntity
    {
        public string OriginalPassword { get; set; }
    }
}