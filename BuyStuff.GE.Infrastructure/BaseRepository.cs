using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuyStuff.GE.Infrastructure
{
    public abstract class BaseRepository<T> where T : class
    {

        protected readonly DbContext _context;


        protected readonly DbSet<T> _dbSet;




        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }



        public async Task<List<T>> GetAllAsync(CancellationToken token)
        {
            return await _dbSet.ToListAsync(token);
        }

        public async Task<T> GetAsync(CancellationToken token, params object[] key)
        {
            return await _dbSet.FindAsync(key, token);
        }

        public async Task AddAsync( T entity, CancellationToken token)
        {
            await _dbSet.AddAsync(entity, token);
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            if (entity == null)
                return;
            _dbSet.Update(entity);

        }

        public async Task RemoveAsync(CancellationToken token, params object[] key)
        {
            var entity = await GetAsync(token, key);
            _dbSet.Remove(entity);
        }


        public Task<bool> AnyAsync(CancellationToken token, Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate, token);
        }

    }
}
