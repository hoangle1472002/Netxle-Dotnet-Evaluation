using Microsoft.EntityFrameworkCore.Storage;
using NexleEvaluation.Domain.Interfaces;

namespace NexleEvaluation.Infrastructure.Transactions
{
    public class EfTransaction : ICustomTransaction
    {
        private IDbContextTransaction _tran;
        public EfTransaction(IDbContextTransaction tran)
        {
            _tran = tran;
        }
        public void Commit()
        {
            _tran.Commit();
        }

        public void Dispose()
        {
            _tran.Dispose();
        }

        public void Rollback()
        {
            _tran.Rollback();
        }
    }
}
