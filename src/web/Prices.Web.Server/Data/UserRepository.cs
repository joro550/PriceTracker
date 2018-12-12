using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Prices.Web.Server.Data.Entities;

namespace Prices.Web.Server.Data
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAll();
        Task<List<UserEntity>> ByPartitionKey(string value);
        Task Add(UserEntity itemEntity);
        Task<UserEntity> GetByUsername(string userUserName);
    }
    
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        public UserRepository(CloudTableClient client)
            : base(client.GetTableReference("users"))
        {
        }

        public Task<UserEntity> GetByUsername(string userUserName)
        {
            throw new System.NotImplementedException();
        }
    }
}