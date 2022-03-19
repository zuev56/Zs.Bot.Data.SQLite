using Microsoft.EntityFrameworkCore;
using System;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.SQLite
{
    public sealed class SQLiteBotContext : BotContext<SQLiteBotContext>
    {
        public SQLiteBotContext()
            : base()
        {
        }

        public SQLiteBotContext(DbContextOptions<SQLiteBotContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureEntities(modelBuilder);
            SeedData(modelBuilder);
        }

        public static void ConfigureEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(b =>
            {
                b.Property(c => c.Id)
                    .ValueGeneratedOnAdd();

                b.Property(c => c.ChatTypeId)
                    .IsRequired()
                    .HasMaxLength(10);

                b.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property(c => c.Description)
                    .HasMaxLength(100);

                b.Property(c => c.RawData)
                    .IsRequired();

                b.Property(c => c.RawDataHash)
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property(c => c.RawDataHistory);

                b.Property(c => c.InsertDate)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("datetime('now')"); // UTC

                b.Property(c => c.UpdateDate)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("datetime('now')"); // UTC

                b.HasKey(c => c.Id);

                b.HasIndex(c => c.ChatTypeId);

                b.ToTable("Chats");
            });

            modelBuilder.Entity<ChatType>(b =>
            {
                b.Property(t => t.Id)
                    .HasMaxLength(10);

                b.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(10);

                b.HasKey(t => t.Id);

                b.ToTable("ChatTypes");
            });

            modelBuilder.Entity<Command>(b =>
            {
                b.Property(c => c.Id)
                    .HasMaxLength(50);

                b.Property(c => c.Group)
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property(c => c.Description)
                    .HasMaxLength(100);

                b.Property(c => c.DefaultArgs)
                    .HasMaxLength(100);

                b.Property(c => c.Script)
                    .IsRequired()
                    .HasMaxLength(5000);

                b.HasKey(c => c.Id);

                b.ToTable("Commands");
            });

            modelBuilder.Entity<Message>(b =>
            {
                b.Property(m => m.Id)
                    .ValueGeneratedOnAdd();

                b.Property(m => m.ChatId);

                b.Property(m => m.UserId);

                b.Property(m => m.MessageTypeId)
                    .IsRequired()
                    .HasMaxLength(3);

                b.Property(m => m.MessengerId)
                    .IsRequired()
                    .HasMaxLength(2);

                b.Property(m => m.ReplyToMessageId);

                b.Property(m => m.Text)
                    .HasMaxLength(100);

                b.Property(m => m.IsSucceed);

                b.Property(m => m.IsDeleted);

                b.Property(m => m.RawData)
                    .IsRequired();

                b.Property(m => m.RawDataHash)
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property(m => m.RawDataHistory);

                b.Property(m => m.FailDescription);

                b.Property(m => m.FailsCount);

                b.Property(c => c.InsertDate)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("datetime('now')"); // UTC

                b.Property(c => c.UpdateDate)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("datetime('now')"); // UTC

                b.HasKey(m => m.Id);

                b.HasIndex(m => m.ChatId);

                b.HasIndex(m => m.MessageTypeId);

                b.HasIndex(m => m.MessengerId);

                b.HasIndex(m => m.ReplyToMessageId);

                b.HasIndex(m => m.UserId);

                b.ToTable("Messages");
            });

            modelBuilder.Entity<MessageType>(b =>
            {
                b.Property(t => t.Id)
                    .HasMaxLength(3);

                b.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                b.HasKey(t => t.Id);

                b.ToTable("MessageTypes");
            });

            modelBuilder.Entity<MessengerInfo>(b =>
            {
                b.Property(i => i.Id)
                    .HasMaxLength(2);

                b.Property(i => i.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                b.HasKey(i => i.Id);

                b.ToTable("MessengerInfos");
            });

            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.Id)
                    .ValueGeneratedOnAdd();

                b.Property(u => u.UserRoleId)
                    .IsRequired()
                    .HasMaxLength(10);

                b.Property(u => u.Name)
                    .HasMaxLength(50);

                b.Property(u => u.FullName)
                    .HasMaxLength(50);

                b.Property(u => u.IsBot);

                b.Property(u => u.RawData)
                    .IsRequired();

                b.Property(u => u.RawDataHash)
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property(u => u.RawDataHistory);

                b.Property(c => c.InsertDate)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("datetime('now')"); // UTC

                b.Property(c => c.UpdateDate)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("datetime('now')"); // UTC

                b.HasKey(u => u.Id);

                b.HasIndex(u => u.UserRoleId);

                b.ToTable("Users");
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.Property(r => r.Id)
                    .HasMaxLength(10);

                b.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property(r => r.Permissions)
                    .IsRequired();

                b.HasKey(r => r.Id);

                b.ToTable("UserRoles");
            });
        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessengerInfo>().HasData(new[]
            {
                new MessengerInfo() { Id = "TG", Name = "Telegram" },
                new MessengerInfo() { Id = "VK", Name = "Вконтакте" },
                new MessengerInfo() { Id = "SK", Name = "Skype" },
                new MessengerInfo() { Id = "FB", Name = "Facebook" },
                new MessengerInfo() { Id = "DC", Name = "Discord" }
            });

            modelBuilder.Entity<ChatType>().HasData(new[]
            {
                new ChatType() { Id = "CHANNEL", Name = "Channel" },
                new ChatType() { Id = "GROUP", Name = "Group" },
                new ChatType() { Id = "PRIVATE", Name = "Private" },
                new ChatType() { Id = "UNDEFINED", Name = "Undefined" }
            });

            modelBuilder.Entity<Chat>().HasData(new[]
            {
                new Chat() { Id = -1, Name = "IntegrationTestChat", Description = "IntegrationTestChat", ChatTypeId = "PRIVATE", RawData = "{ \"test\": \"test\" }", RawDataHash = "-1063294487", InsertDate = DateTime.UtcNow },
                new Chat() { Id = 1, Name = "zuev56", ChatTypeId = "PRIVATE", RawData = "{ \"Id\": 210281448 }", RawDataHash = "-1063294487", InsertDate = DateTime.UtcNow }
            });

            modelBuilder.Entity<UserRole>().HasData(new[]
            {
                new UserRole() { Id = "OWNER", Name = "Owner", Permissions = "[ \"All\" ]" },
                new UserRole() { Id = "ADMIN", Name = "Administrator", Permissions = "[ \"adminCmdGroup\", \"moderatorCmdGroup\", \"userCmdGroup\" ]" },
                new UserRole() { Id = "MODERATOR", Name = "Moderator", Permissions = "[ \"moderatorCmdGroup\", \"userCmdGroup\" ]" },
                new UserRole() { Id = "USER", Name = "User", Permissions = "[ \"userCmdGroup\" ]" }
            });

            modelBuilder.Entity<User>().HasData(new[]
            {
                new User() { Id = -10, Name = "Unknown", FullName = "for exported message reading", UserRoleId = "USER", IsBot = false, RawData = "{ \"test\": \"test\" }", RawDataHash = "-1063294487", InsertDate = DateTime.UtcNow },
                new User() { Id = -1, Name = "IntegrationTestUser", FullName = "IntegrationTest", UserRoleId = "USER", IsBot = false, RawData = "{ \"test\": \"test\" }", RawDataHash = "-1063294487", InsertDate = DateTime.UtcNow },
                new User() { Id = 1, Name = "zuev56", FullName = "Сергей Зуев", UserRoleId = "OWNER", IsBot = false, RawData = "{ \"Id\": 210281448 }", RawDataHash = "-1063294487", InsertDate = DateTime.UtcNow }
            });

            modelBuilder.Entity<MessageType>().HasData(new[]
            {
                new MessageType() { Id = "UKN", Name = "Unknown" },
                new MessageType() { Id = "TXT", Name = "Text" },
                new MessageType() { Id = "PHT", Name = "Photo" },
                new MessageType() { Id = "AUD", Name = "Audio" },
                new MessageType() { Id = "VID", Name = "Video" },
                new MessageType() { Id = "VOI", Name = "Voice" },
                new MessageType() { Id = "DOC", Name = "Document" },
                new MessageType() { Id = "STK", Name = "Sticker" },
                new MessageType() { Id = "LOC", Name = "Location" },
                new MessageType() { Id = "CNT", Name = "Contact" },
                new MessageType() { Id = "SRV", Name = "Service message" },
                new MessageType() { Id = "OTH", Name = "Other" }
            });

            modelBuilder.Entity<Command>().HasData(new[]
            {
                new Command() { Id = "/Test".ToLowerInvariant(), Script = "SELECT 'Test'", Description = "Тестовый запрос к боту. Возвращает ''Test''", Group = "moderatorCmdGroup" },
                new Command() { Id = "/NullTest".ToLowerInvariant(), Script = "SELECT null", Description = "Тестовый запрос к боту. Возвращает NULL", Group = "moderatorCmdGroup" },
                new Command() { Id = "/Help".ToLowerInvariant(), Script = "SELECT 'Not implemented for SQLite :('", DefaultArgs = "<UserRoleId>", Description = "Получение справки по доступным функциям", Group = "userCmdGroup" },
                new Command() { Id = "/SqlQuery".ToLowerInvariant(), Script = "SELECT 'Not implemented for SQLite :('", DefaultArgs = "select 'Pass your query as a parameter in double quotes'", Description = "SQL-запрос", Group = "adminCmdGroup" }
            });
        }

        public static string GetOtherSqlScripts(string configPath)
        {

            // TODO:

            throw new NotImplementedException("Nothing for SQLite");
        }
    }
}
