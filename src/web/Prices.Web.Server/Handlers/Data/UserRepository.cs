using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Handlers.Data
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAll();
        Task<List<UserEntity>> ByPartitionKey(string value);
        Task<bool> Add(UserEntity itemEntity);
        Task<UserEntity> GetByUsername(string userUserName);
        Task<UserEntity> GetById(string userId);
    }
    
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        public UserRepository(CloudTableClient client)
            : base(client.GetTableReference("users"))
        {
        }

        public async Task<UserEntity> GetByUsername(string userUserName)
        {
            var tableQuery = new TableQuery<UserEntity>()
                .Where(TableQuery.GenerateFilterCondition(nameof(UserEntity.Username), QueryComparisons.Equal, userUserName));
            var results = await TableClient.ExecuteQuerySegmentedAsync(tableQuery, new TableContinuationToken());
            return results.Results.SingleOrDefault();
        }

        public async Task<UserEntity> GetById(string userId)
        {
            var tableQuery = new TableQuery<UserEntity>()
                .Where(TableQuery.GenerateFilterCondition(nameof(UserEntity.Id), QueryComparisons.Equal, userId));
            var results = await TableClient.ExecuteQuerySegmentedAsync(tableQuery, new TableContinuationToken());
            return results.Results.SingleOrDefault();
        }
    }
}