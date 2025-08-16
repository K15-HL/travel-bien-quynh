using Microsoft.Extensions.Options;
using MongoDB.Driver;
using travel_bien_quynh.Options;

namespace travel_bien_quynh.Contexts
{
    public class MongoDbContext : IMongoDbContext
    {
        private IMongoDatabase _db;
        private MongoClient _client;

        public MongoDbContext(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            _db = _client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
