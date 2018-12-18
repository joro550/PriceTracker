using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Prices.Web.Server.Handlers.Data;

namespace Prices.Web.Server.Tests.Fakes
{
    public abstract class InMemoryRepository<T> : IRepository<T> where T : TableEntity, new()
    {
        protected readonly List<T> Items;

        protected InMemoryRepository(List<T> items) 
            => Items = items;

        public Task<List<T>> GetAll() 
            => Task.FromResult(Items);

        public Task<List<T>> ByPartitionKey(string value) 
            => Task.FromResult(Items.Where(item => item.PartitionKey == value).ToList());

        public Task<bool> Add(T item) 
            => Task.Run(() =>
            {
                Items.Add(item);
                return true;
            });
    }
}