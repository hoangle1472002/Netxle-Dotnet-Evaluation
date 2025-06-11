using System.Threading.Tasks;
using System.Transactions;

namespace NexleEvaluation.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomTransaction BeginTransaction(IsolationLevel level);
        ICustomTransaction BeginTransaction();
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
