using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly OrderContext dbContext;

        public RepositoryBase(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
