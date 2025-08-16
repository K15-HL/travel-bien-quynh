using travel_bien_quynh.Entities;

namespace travel_bien_quynh.Repositories.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class, IMongoEntity
    {
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity> GetAsync(string id);
        Task<TEntity> GetAsyncByField(string feild, string value);
        Task CreateAsync(TEntity obj);
        Task UpdateAsync(string id, TEntity obj);
        Task DeleteAsync(string id);
        Task DeleteAllAsync();
    }
}
