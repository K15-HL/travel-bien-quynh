using travel_bien_quynh.Contexts;
using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories.Base;
using travel_bien_quynh.Repositories.Interface;
using MongoDB.Driver;

namespace travel_bien_quynh.Repositories
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(IMongoDbContext mongoContext) : base(mongoContext)
        {

        }
    }
}
