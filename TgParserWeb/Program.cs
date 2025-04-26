using Microsoft.EntityFrameworkCore;
using TgParser.Data;
using TgParser.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMessageLogger, MessageLogger>();
builder.Services.AddScoped<IMessageProcessor, MessageProcessor>();
builder.Services.AddScoped<IChannelParser, ChannelParser>();
builder.Services.AddScoped<ConfigService>();
builder.Services.AddScoped<TelegramService>();

var app = builder.Build();

// Использование статических файлов
app.UseStaticFiles();

// Маршрутизация для Razor Pages
app.MapRazorPages();

// Маршрутизация для контроллеров API
app.MapControllers();

app.Run();
