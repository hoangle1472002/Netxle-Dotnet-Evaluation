using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NexleEvaluation.Domain.Interfaces;
using NexleEvaluation.Infrastructure.Context;
using System.Collections.Generic;

namespace NexleEvaluation.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T>, IRepository where T : class
    {
        protected DbSet<T> Set { get; set; }
        protected internal AppDbContext Context { get; set; }

        public Type EntityType { get; set; } = typeof(T);

        public Repository(AppDbContext context)
        {
            Context = context;
            Set = context.Set<T>();
        }

        public T Add(T entity)
        {
            if (Set.Local.Any(p => p == entity))
            {
                return entity;
            }
            var result = Set.Add(entity);
            return result.Entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            if (Set.Local.Any(p => p == entity))
            {
                return entity;
            }
            var addedItem = await Set.AddAsync(entity);
            return addedItem.Entity;
        }

        public object Add(object entity)
        {
            return Add(entity as T);
        }


        public object Delete(object entity)
        {
            return Delete(entity as T);
        }

        public T Update(T entity)
        {
            if (Set.Local.Any(p => p == entity))
            {
                return entity;
            }
            return Set.Attach(entity).Entity;
        }

        public object Update(object entity)
        {
            return Update(entity as T);
        }

        public T Find(params object[] keys)
        {
            var result = Set.Find(keys);
            return result;
        }

        public async Task<T> FindAsync(params object[] keys)
        {
            var result = await Set.FindAsync(keys);
            return result;
        }

        public IQueryable<T> GetAll()
        {
            return Set;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Set.Where(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await Set.FirstOrDefaultAsync(predicate);
            return result;
        }

        public virtual IQueryable<T> GetAll(bool ignoreGlobalFilter)
        {
            return GetAll();
        }

        public T HardDelete(T entity)
        {
            Set.Remove(entity);
            return entity;
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Set.RemoveRange(entities);
        }

    }
}
