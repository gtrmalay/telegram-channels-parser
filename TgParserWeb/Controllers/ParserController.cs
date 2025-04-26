using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TgParser.Data;
using TgParser.Services;
using System.Diagnostics;

[Route("api/parser")]
[ApiController]
public class ParserController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TelegramService _telegramService;
    private readonly ILogger<ParserController> _logger;

    public ParserController(
        AppDbContext db,
        TelegramService telegramService,
        ILogger<ParserController> logger)
    {
        _db = db;
        _telegramService = telegramService;
        _logger = logger;
    }

    [HttpPost("import-news")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartImport()
    {
        try
        {
            _logger.LogInformation("Запуск импорта новостей");

            await _telegramService.LoginAsync();
            _logger.LogInformation("Успешный логин в Telegram");

            var channelsToMonitor = AppConfig.GetChannelsToMonitor();
            _logger.LogInformation($"Каналы для мониторинга: {string.Join(", ", channelsToMonitor)}");

            await _telegramService.MonitorChannelsAsync(channelsToMonitor, _db);
            _logger.LogInformation("Мониторинг каналов завершен");

            return Ok(new { success = true, message = "Импорт успешно запущен" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при запуске импорта");
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    
    [HttpGet("news")]
    public async Task<IActionResult> GetNews([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var query = _db.RawNews.AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<RawNewsModel>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении новостей");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
