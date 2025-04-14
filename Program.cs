using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TgParser.Data;
using TgParser.Services;

class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        try
        {
            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Host=localhost;Database=raw_news;Username=postgres;Password=Ebds777staX")
                .Options;

            await using var db = new AppDbContext(dbOptions);

            var telegramService = serviceProvider.GetRequiredService<TelegramService>();
            await telegramService.LoginAsync();
            await telegramService.MonitorChannelsAsync(AppConfig.GetChannelsToMonitor(), db);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Используем AppConfig.GetTelegramConfigValue как Func<string, string>
        services.AddSingleton<Func<string, string>>(_ => AppConfig.GetTelegramConfigValue);

        services.AddTransient<IMessageLogger, MessageLogger>();
        services.AddTransient<IMessageProcessor, MessageProcessor>();
        services.AddTransient<IChannelParser, ChannelParser>();
        services.AddTransient<TelegramService>();
    }
}