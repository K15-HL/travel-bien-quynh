using travel_bien_quynh.Contexts;
using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories.Base;
using travel_bien_quynh.Repositories.Interface;

namespace travel_bien_quynh.Repositories
{
    public class LogWalletRepository : BaseRepository<LogWallet>, ILogWallet
    {
        public LogWalletRepository(IMongoDbContext mongoContext) : base(mongoContext)
        {
        }
    }

}
