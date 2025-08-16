using MongoDB.Driver;

namespace travel_bien_quynh.Contexts
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
