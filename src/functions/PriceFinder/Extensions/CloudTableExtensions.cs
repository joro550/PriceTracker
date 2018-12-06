using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PriceFinder.Extensions
{
    public static class CloudTableExtensions 
    {
        public static async Task<TableResult> InsertEntity(this CloudTable cloudTable, ITableEntity entity) 
            => await cloudTable.ExecuteAsync(TableOperation.Insert(entity));
    }
}