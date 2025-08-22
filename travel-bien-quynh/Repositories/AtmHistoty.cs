using travel_bien_quynh.Contexts;
using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories.Base;
using travel_bien_quynh.Repositories.Interface;

namespace travel_bien_quynh.Repositories
{
    public class AtmHistoty : BaseRepository<AtmHistory>, IAtmHistory
    
    {
        public AtmHistoty(IMongoDbContext mongoContext) : base(mongoContext)
        {
        }
    }
    
}
