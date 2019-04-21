using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using OnlineToyStore.Core.Interfaces;
using OnlineToyStore.Core.Models;

namespace OnlineToyStore.Core.Repositories
{
    public class OrdersRepository<T> : IOrdersRepository<Order>
    {
        private readonly string Endpoint = "https://onlinetoystoredemo.documents.azure.com:443/";
        private readonly string Key = "dSMYzEl7rWGIjMWhYzQW9bVk1po4elhrWMBlxXDKpwGZCAlYzMQjgXEj2OJKcHP31aVlcGBQgB2uia4pnZ1kDw==";
        private readonly string DatabaseId = "OnlineToyStore";
        private readonly string CollectionId = "Orders";
        private DocumentClient client;

        public OrdersRepository()
        {
            var option = new FeedOptions { EnableCrossPartitionQuery = true };
            this.client = new DocumentClient(new Uri(Endpoint), Key);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public async Task<Order> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (Order)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<KeyValuePair<string, IEnumerable<Order>>> GetItemsAsync(Expression<Func<Order, bool>> predicate, int requestSize, string continuationToken)
        {
            if (string.IsNullOrEmpty(continuationToken))
            {
                IDocumentQuery<Order> query = client.CreateDocumentQuery<Order>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions
                {
                    MaxItemCount = requestSize,
                    EnableCrossPartitionQuery = false
                })
                //.AsExpandable()
                .Where(predicate)
                .AsDocumentQuery();

                FeedResponse<Order> feedRespose = await query.ExecuteNextAsync<Order>();

                List<Order> results = new List<Order>();
                foreach (var t in feedRespose.AsEnumerable())
                {
                    results.Add(t);
                }

                return new KeyValuePair<string, IEnumerable<Order>>(feedRespose.ResponseContinuation, results);

            }
            else
            {
                IDocumentQuery<Order> query = client.CreateDocumentQuery<Order>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions
                {
                    MaxItemCount = requestSize,
                    EnableCrossPartitionQuery = false,
                    RequestContinuation = continuationToken
                })
                .Where(predicate)
                .AsDocumentQuery();

                FeedResponse<Order> feedRespose = await query.ExecuteNextAsync<Order>();

                List<Order> results = new List<Order>();
                foreach (var t in feedRespose.AsEnumerable())
                {
                    results.Add(t);
                }

                return new KeyValuePair<string, IEnumerable<Order>>(feedRespose.ResponseContinuation, results);

            }

        }

        public async Task<Document> CreateItemAsync(Order item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public async Task<Document> UpdateItemAsync(string id, Order item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

    }
}
