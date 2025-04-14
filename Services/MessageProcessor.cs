using TgParser.Data;
using TL;

namespace TgParser.Services
{
    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(AppDbContext db, Channel channel, Message message);
    }

    public class MessageProcessor : IMessageProcessor
    {
        private readonly IMessageLogger _messageLogger;

        public MessageProcessor(IMessageLogger messageLogger)
        {
            _messageLogger = messageLogger;
        }

        public async Task ProcessMessageAsync(AppDbContext db, Channel channel, Message message)
        {
            try
            {
                var newsItem = CreateNewsItem(channel, message);
                await SaveMessageToDb(db, newsItem);
                _messageLogger.LogMessage(channel, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки: {ex.Message}");
            }
        }

        private RawNewsModel CreateNewsItem(Channel channel, Message message)
        {
            var fullText = message.message ?? string.Empty;

            return new RawNewsModel
            {
                Source = channel.title ?? "Unknown",
                Title = GetShortTitle(fullText),
                Text = fullText,
                Link = $"https://t.me/{channel.username}/{message.id}",
                Guid = message.id.ToString(),
                Author = channel.username ?? channel.title ?? "Unknown",
                CreatedAt = DateTime.UtcNow,
                IsProcessed = false
            };
        }

        private string GetShortTitle(string fullText)
        {
            return fullText.Length > 70
                ? fullText.Substring(0, 70) + "..."
                : fullText;
        }

        private async Task SaveMessageToDb(AppDbContext db, RawNewsModel newsItem)
        {
            await db.RawNews.AddAsync(newsItem);
            await db.SaveChangesAsync();
            Console.WriteLine($"Сохранено. Длина текста: {newsItem.Text.Length}");
        }
    }
}