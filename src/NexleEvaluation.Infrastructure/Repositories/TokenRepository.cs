using Microsoft.EntityFrameworkCore;
using NexleEvaluation.Domain.Entities;
using NexleEvaluation.Domain.Interfaces;
using NexleEvaluation.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexleEvaluation.Infrastructure.Repositories
{
    public class TokenRepository : Repository<Token>, ITokenRepository
    {
        private readonly AppDbContext _context;
        public TokenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Token> GetWithUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Tokens
                                 .Include(t => t.User)
                                 .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        }

        public async Task<List<Token>> GetByUserIdAsync(int userId)
        {
            return await _context.Tokens
                                 .Where(t => t.UserId == userId)
                                 .ToListAsync();
        }
    }
}

