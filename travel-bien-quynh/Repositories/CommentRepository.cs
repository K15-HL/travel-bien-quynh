using travel_bien_quynh.Contexts;
using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories.Base;
using travel_bien_quynh.Repositories.Interface;
using MongoDB.Driver;

namespace travel_bien_quynh.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IMongoDbContext mongoContext) : base(mongoContext)
        {

        }
    }
}
