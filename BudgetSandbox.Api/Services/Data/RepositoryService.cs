using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using System.Linq.Expressions;

namespace BudgetSandbox.Api.Services.Data
{
    public interface IRepositoryService<TEntity> where TEntity : class
    {
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(TEntity entity);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, params string[] navigationProperties);
        Task<List<TEntity>> GetMultipleAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, params string[] navigationProperties);
        Task AddOrUpdateAsync(TEntity entity);
    }

    public class RepositoryService<TEntity> : IRepositoryService<TEntity> where TEntity : class
    {
        private readonly BudgetSandboxContext _context;

        public RepositoryService(BudgetSandboxContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, params string[] navigationProperties)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (tracking)
            {
                query = query.AsNoTracking().Where(predicate);
            }
            else
            {
                query = query.Where(predicate);
            }

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetMultipleAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, params string[] navigationProperties)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            
            if(tracking)
            {
                query = query.Where(predicate);
            }
            else
            {
                query = query.AsNoTracking().Where(predicate);
            }

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            return await query.ToListAsync();
        }

        public async Task AddOrUpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}
