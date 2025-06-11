using NexleEvaluation.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexleEvaluation.Domain.Interfaces
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task<Token> GetWithUserByRefreshTokenAsync(string refreshToken);
        Task<List<Token>> GetByUserIdAsync(int userId);
    }
}
