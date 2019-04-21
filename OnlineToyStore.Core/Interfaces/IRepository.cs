using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineToyStore.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Document> CreateItemAsync(T item);
        Task DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<KeyValuePair<string, IEnumerable<T>>> GetItemsAsync(Expression<Func<T, bool>> predicate, int requestSize, string continuationToken);
        Task<Document> UpdateItemAsync(string id, T item);
    }
}
