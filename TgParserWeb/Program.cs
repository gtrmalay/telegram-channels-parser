using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TgParser.Data;
using TgParser.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var currentDirectory = Directory.GetCurrentDirectory();

var configuration = new ConfigurationBuilder()
    .SetBasePath(currentDirectory)  
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
    .Build();

builder.Services.AddLogging(configure => configure
    .AddConsole()  
    .AddDebug()); 

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));  

builder.Services.AddScoped<IMessageLogger, MessageLogger>();
builder.Services.AddScoped<IMessageProcessor, MessageProcessor>();
builder.Services.AddScoped<IChannelParser, ChannelParser>();

builder.Services.AddScoped<TelegramService>(serviceProvider =>
{
    var messageProcessor = serviceProvider.GetRequiredService<IMessageProcessor>();
    var channelParser = serviceProvider.GetRequiredService<IChannelParser>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();  

    return new TelegramService(messageProcessor, channelParser, configuration);  
});

var app = builder.Build();


app.UseStaticFiles();
app.MapRazorPages();


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await AppConfig.InitializeAsync(dbContext);  
}

app.Run();
