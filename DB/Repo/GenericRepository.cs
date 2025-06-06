using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ClothingStoreDbContext context;
        protected DbSet<T> set;

        public GenericRepository(ClothingStoreDbContext context)
        {
            this.context = context;
            set = this.context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await set.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await set.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await set.AddAsync(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            set.Update(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            set.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await set.Where(predicate).ToListAsync();
        }
    }
}