using TL;
using WTelegram;

namespace TgParser.Services
{
    public interface IChannelParser
    {
        Task<Channel> ResolveChannelAsync(Client client, string username);
        Task<Message> GetLastMessageAsync(Client client, Channel channel);
    }

    public class ChannelParser : IChannelParser
    {
        public async Task<Channel> ResolveChannelAsync(Client client, string username)
        {
            var resolved = await client.Contacts_ResolveUsername(username);

            if (resolved.chats.TryGetValue(resolved.peer.ID, out var chat) && chat is Channel channel)
            {
                Console.WriteLine($"Найден канал: {channel.title}");
                return channel;
            }

            Console.WriteLine($"Канал @{username} не найден");
            return null;
        }

        public async Task<Message> GetLastMessageAsync(Client client, Channel channel)
        {
            var history = await client.Messages_GetHistory(channel, limit: AppConfig.LAST_POSTS_FROM_CHANNEL_COUNT);
            return history.Messages[0] as Message;
        }
    }
}