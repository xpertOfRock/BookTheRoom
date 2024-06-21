using BookTheRoom.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        public BaseRepository(DbContext context)
        {
            _context = context;
        }
        public Task Add(TEntity entity)
        {
            _context.Set<TEntity>().AddAsync(entity);
            return Task.CompletedTask;
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }


        public virtual void Update(TEntity entity)
        {
            _context.Update(entity);           
        }
    }
}
