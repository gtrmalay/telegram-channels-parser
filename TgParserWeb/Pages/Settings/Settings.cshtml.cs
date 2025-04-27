using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using TgParser.Data;

public class SettingsPageModel : PageModel
{
    private readonly AppDbContext _dbContext;

    public SettingsPageModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public string SessionName { get; set; }
    [BindProperty]
    public List<string> Channels { get; set; } = new List<string>();
    [BindProperty]
    public int MessageLimit { get; set; }
    [BindProperty]
    public DateTime ParseStartDate { get; set; }
    [BindProperty]
    public float DuplicateThreshold { get; set; }
    [BindProperty]
    public float AdThreshold { get; set; }

    public void OnGet()
    {
        // Загрузка текущих настроек из базы данных, если они есть
        var settings = _dbContext.Settings.FirstOrDefault();
        if (settings != null)
        {
            SessionName = settings.SessionName;
            if (!string.IsNullOrEmpty(settings.Channels))
            {
                Channels = settings.Channels.Split(',').ToList();
            }
            else
            {
                Channels = new List<string>(); 
            }

            MessageLimit = settings.MessageLimit;
            ParseStartDate = settings.ParseStartDate;
            DuplicateThreshold = settings.DuplicateThreshold;
            AdThreshold = settings.AdThreshold;
        }
    }

    public IActionResult OnPost()
    {
        // Найти настройки, если они уже есть
        var settings = _dbContext.Settings.FirstOrDefault();
        if (settings == null)
        {
            // Если настроек нет, создаем новые
            settings = new SettingsModel
            {
                SessionName = SessionName,
                Channels = string.Join(",", Channels),
                MessageLimit = MessageLimit,
                ParseStartDate = ParseStartDate,
                DuplicateThreshold = DuplicateThreshold,
                AdThreshold = AdThreshold
            };
            _dbContext.Settings.Add(settings);
        }
        else
        {
            // Если настройки есть, обновляем их
            settings.SessionName = SessionName;
            settings.Channels = string.Join(",", Channels);
            settings.MessageLimit = MessageLimit;
            settings.ParseStartDate = ParseStartDate;
            settings.DuplicateThreshold = DuplicateThreshold;
            settings.AdThreshold = AdThreshold;
        }

        // Сохраняем изменения в базе данных
        _dbContext.SaveChanges();

        // Переадресация на страницу настроек с успешным сохранением
        return RedirectToPage("/Index");
    }
}
