using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Zs.Bot.Data.Models;
using Zs.Bot.Data.Repositories;

namespace Zs.Bot.Data.SQLite.Repositories
{
    public sealed class ChatsRepository<TContext> : ChatsRepositoryBase<TContext>
        where TContext : DbContext
    {
        public ChatsRepository(
            IDbContextFactory<TContext> contextFactory,
            TimeSpan? criticalQueryExecutionTimeForLogging = null,
            ILogger<ChatsRepository<TContext>> logger = null)
            : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
        {
        }

        public override async Task<Chat> FindByRawDataIdAsync(long rawId)
        {
            return await FindBySqlAsync($"select * from Chats where json_extract(Chats.RawData, '$.Id') = {rawId}").ConfigureAwait(false);
        }
    }
}
