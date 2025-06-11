using Microsoft.EntityFrameworkCore;
using NexleEvaluation.Domain.Entities;
using NexleEvaluation.Domain.Entities.Base;
using NexleEvaluation.Domain.Entities.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexleEvaluation.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("token");
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker
                                       .Entries()
                                       .Where(e => e.Entity is IBaseEntity && (
                                            e.State == EntityState.Added
                                            || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((IBaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
                ((IBaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}