using TgParser.Data;
using TgParser.Extensions;
using TL;
using WTelegram;

namespace TgParser.Services
{
    public class TelegramService : IDisposable
    {
        private readonly Client _client;
        private readonly IMessageProcessor _messageProcessor;
        private readonly IChannelParser _channelParser;

        public TelegramService(
            Func<string, string> configProvider,
            IMessageProcessor messageProcessor,
            IChannelParser channelParser)
        {
            _client = new Client(configProvider);
            _messageProcessor = messageProcessor;
            _channelParser = channelParser;
        }

        public async Task<User> LoginAsync()
        {
            var user = await _client.LoginUserIfNeeded();
            Console.WriteLine($"Авторизация успешна. Пользователь: {user.GetDisplayName()}");
            return user;
        }

        public async Task MonitorChannelsAsync(IEnumerable<string> channelUsernames, AppDbContext db)
        {
            foreach (var username in channelUsernames)
            {
                await ProcessChannelAsync(username, db);
            }
        }

        private async Task ProcessChannelAsync(string username, AppDbContext db)
        {
            Console.WriteLine($"\n[Обработка канала @{username}]");

            try
            {
                var channel = await _channelParser.ResolveChannelAsync(_client, username);
                if (channel == null) return;

                var message = await _channelParser.GetLastMessageAsync(_client, channel);
                if (message != null)
                {
                    await _messageProcessor.ProcessMessageAsync(db, channel, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}