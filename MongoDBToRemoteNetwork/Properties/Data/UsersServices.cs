using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MongoDBToRemoteNetwork.Properties.Data
{
    public class UsersServices
    {
        protected readonly IMongoCollection<Users> _usersCollection;

        public UsersServices(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<Users>(
            bookStoreDatabaseSettings.Value.UsersCollectionName);
        }
        public async Task<List<Users>> GetUsersAsync() =>
         await _usersCollection.Find(_ => true).ToListAsync();
        public async Task CreateUserAsync(Users newUsers) =>
            await _usersCollection.InsertOneAsync(newUsers);
        public async Task<Users?> GetAsyncUseremail(string email) =>
           await _usersCollection.Find(x => x.Email == email).FirstOrDefaultAsync();
        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
        public async Task<Users?> GetAsync(string id) =>
          await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    }
}
