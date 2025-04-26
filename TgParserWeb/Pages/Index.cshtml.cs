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
        

    }
}