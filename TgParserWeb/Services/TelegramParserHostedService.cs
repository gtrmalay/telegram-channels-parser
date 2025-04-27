using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TgParser.Data;
using TgParser.Services;

namespace TgParserWeb.Services
{
    public class TelegramParserHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<TelegramParserHostedService> _logger;
        private Timer? _timer;

        public TelegramParserHostedService(
            IServiceProvider services,
            ILogger<TelegramParserHostedService> logger)
        {
            _services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parser Service running");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state) 
        {
            try
            {
                using var scope = _services.CreateScope();
                var parser = scope.ServiceProvider.GetRequiredService<TelegramService>();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await parser.LoginAsync();
                await parser.MonitorChannelsAsync(AppConfig.GetChannelsToMonitor(), db);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TelegramParserHostedService");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parser Service is stopping");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}