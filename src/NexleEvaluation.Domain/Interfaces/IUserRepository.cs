using System.Threading.Tasks;
using NexleEvaluation.Domain.Entities;
using NexleEvaluation.Domain.Entities.Identity;

namespace NexleEvaluation.Domain.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<User> GetByEmailAsync(string email);
    }
}

