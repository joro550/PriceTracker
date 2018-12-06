using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Prices.Web.Server.Data;

namespace Prices.Web.Server.Tests.Fakes
{
    public abstract class BaseRepository<T> : IRepository<T> where T : TableEntity, new()
    {
        private readonly List<T> _items;

        protected BaseRepository(List<T> items)
        {
            _items = items;
        }

        public Task<List<T>> GetAll()
        {
            return Task.FromResult(_items);
        }

        public Task<List<T>> ByPartitionKey(string value)
        {
            return Task.FromResult(_items.Where(item => item.PartitionKey == value).ToList());
        }

        public Task Add(T item)
        {
            return Task.Run(() => _items.Add(item));
        }
    }
}