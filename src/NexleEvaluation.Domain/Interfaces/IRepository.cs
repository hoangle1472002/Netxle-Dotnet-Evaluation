using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NexleEvaluation.Domain.Interfaces
{
    public interface IRepository
    {
        object Add(object entity);
        object Update(object entity);
        object Delete(object entity);
        Type EntityType { get; }
    }

    public interface IRepository<T> : IRepository
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(bool ignoreGlobalFilter);

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        T HardDelete(T entity);
        T Find(params object[] keys);
        Task<T> FindAsync(params object[] keys);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        void RemoveRange(IEnumerable<T> entities);
    }
}
