using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zs.Bot.Data.Models;
using Zs.Bot.Data.Repositories;

namespace Zs.Bot.Data.SQLite.Repositories
{
#warning Depends on Telegram
    public sealed class MessagesRepository<TContext> : MessagesRepositoryBase<TContext>
        where TContext : DbContext
    {
        public MessagesRepository(
            IDbContextFactory<TContext> contextFactory,
            TimeSpan? criticalQueryExecutionTimeForLogging = null,
            ILogger<MessagesRepository<TContext>> logger = null)
            : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
        {
        }

        public override async Task<Message> FindByRawDataIdsAsync(int rawMessageId, long rawChatId)
        {
            // TODO: Check telegram message update and correct query
            return await FindBySqlAsync(
                  $"select * from Messages "
                + $"where json_extract(Messages.RawData, '$.MessageId') = {rawMessageId}"
                + $"  and json_extract(Messages.RawData, '$.ChatId') = {rawChatId}").ConfigureAwait(false);
        }

        public override async Task<List<Message>> FindDailyMessages(int chatId)
        {
            return await FindAllBySqlAsync(
                  $"select * from Messages "
                + $"where ChatId = {chatId} and json_extract(Messages.RawData, '$.Date') > date('now', 'localtime')").ConfigureAwait(false);
        }

        public override async Task<Dictionary<int, int>> FindUserIdsAndMessagesCountSinceDate(int chatId, DateTime? startDate)
        {
            var userIdsAndMessageCounts = new Dictionary<int, int>();

            if (startDate != null)
            {
                var selectChatMessagesSinceDate =
                      $"select * from Messages "
                    + $"where ChatId = {chatId} "
                    + $"  and json_extract(Messages.RawData, '$.Date') > date('now', 'localtime') "  // ???
                    + $"  and json_extract(Messages.RawData, '$.Date') > '{startDate:yyyyMMddHHmmss}'";

                var messagesSinceDate = await FindAllBySqlAsync(selectChatMessagesSinceDate).ConfigureAwait(false);

                foreach (var messageGroup in messagesSinceDate.GroupBy(m => m.UserId))
                {
                    userIdsAndMessageCounts.Add(messageGroup.Key, messageGroup.Count());
                }     
            }

            return userIdsAndMessageCounts;
        }
    }
}
