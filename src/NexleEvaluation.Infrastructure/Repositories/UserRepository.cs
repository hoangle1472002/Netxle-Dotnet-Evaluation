using Microsoft.EntityFrameworkCore;
using NexleEvaluation.Domain.Entities.Identity;
using NexleEvaluation.Domain.Interfaces;
using NexleEvaluation.Infrastructure.Context;
using System.Threading.Tasks;

namespace NexleEvaluation.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }

}

