using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TgParser.Data;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public bool IsAuthenticated { get; set; }
    public int TodayCount { get; set; }
    public int DuplicatesCount { get; set; }
    public int AdsCount { get; set; }

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public async Task OnGetAsync()
    {
        DateTime someDate = DateTime.UtcNow;
        var result = await _db.RawNews
            .Where(news => news.CreatedAt.Date == someDate.Date)
            .CountAsync();
        TodayCount = result;
            

        DuplicatesCount = await _db.RawNews
            .GroupBy(news => new { news.Title, news.Text }) 
            .Where(group => group.Count() > 1) 
            .CountAsync(); 

    }
}