namespace BookTheRoom.Core.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetAll();
        Task Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
