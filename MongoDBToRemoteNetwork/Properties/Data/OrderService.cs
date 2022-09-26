using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MongoDBToRemoteNetwork.Properties.Data
{
    public class OrderService
    {
        protected readonly IMongoCollection<Order> _orderCollection;

        public OrderService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _orderCollection = mongoDatabase.GetCollection<Order>(
            bookStoreDatabaseSettings.Value.OrderCollectionName);
        }
        public async Task<List<Order>> GetOrderAsync() =>
         await _orderCollection.Find(_ => true).ToListAsync();


    }
}
