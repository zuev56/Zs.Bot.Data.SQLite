using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Zs.Bot.Data.Models;
using Zs.Bot.Data.Repositories;

namespace Zs.Bot.Data.SQLite.Repositories
{
    public sealed class UsersRepository<TContext> : UsersRepositoryBase<TContext>
        where TContext : DbContext
    {
        public UsersRepository(
            IDbContextFactory<TContext> contextFactory,
            TimeSpan? criticalQueryExecutionTimeForLogging = null,
            ILogger<UsersRepository<TContext>> logger = null)
            : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
        {
        }

        public override async Task<User> FindByRawDataIdAsync(long rawId)
        {
            return await FindBySqlAsync($"select * from Users where json_extract(Users.RawData, '$.Id') = {rawId}").ConfigureAwait(false);
        }
    }
}
