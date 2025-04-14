using TL;

namespace TgParser.Services
{
    public interface IMessageLogger
    {
        void LogMessage(Channel channel, Message message);
    }

    public class MessageLogger : IMessageLogger
    {
        public void LogMessage(Channel channel, Message message)
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Канал: {channel.title}");
            Console.WriteLine($"ID: {channel.id}");
            Console.WriteLine($"Создан: {channel.date:dd.MM.yyyy}");
            Console.WriteLine($"Последнее сообщение ({message.date:dd.MM.yyyy HH:mm}):");
            Console.WriteLine(message.message);
            Console.WriteLine(new string('-', 50));
        }
    }
}