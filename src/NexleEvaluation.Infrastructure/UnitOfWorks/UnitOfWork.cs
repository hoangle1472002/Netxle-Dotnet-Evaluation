using System.Threading.Tasks;
using System.Transactions;
using NexleEvaluation.Infrastructure.Transactions;
using NexleEvaluation.Infrastructure.Context;
using NexleEvaluation.Domain.Interfaces;

namespace NexleEvaluation.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public ICustomTransaction BeginTransaction(IsolationLevel level)
        {
            return new EfTransaction(_context.Database.BeginTransaction());
        }

        public ICustomTransaction BeginTransaction()
        {
            return new EfTransaction(_context.Database.BeginTransaction());
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

