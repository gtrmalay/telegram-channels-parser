using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TgParser.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using static NewsModel;

namespace TgParserWeb.Pages
{
    public class RawsNewsModel : PageModel
    {
        private readonly AppDbContext _db;

        public RawsNewsModel(AppDbContext db)
        {
            _db = db;
        }

        public List<RawNewsModel> Messages { get; set; } = new();
        public List<string> UniqueSources { get; set; } = new();
        public FilterModel CurrentFilter { get; set; } = new();

        public List<RawNewsModel> RawNewsItems { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public async Task OnGetAsync(string source, DateTime? dateFrom, DateTime? dateTo, int currentPage = 1)
        {
            CurrentPage = currentPage;
            CurrentFilter = new FilterModel { Source = source, DateFrom = dateFrom, DateTo = dateTo };

            var query = _db.RawNews.AsQueryable();

            if (!string.IsNullOrEmpty(source))
                query = query.Where(m => m.Source == source);

            if (dateFrom.HasValue)
                query = query.Where(m => m.CreatedAt >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(m => m.CreatedAt <= dateTo.Value.AddDays(1));

            UniqueSources = await _db.RawNews
                .Select(m => m.Source)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            CurrentPage = Math.Clamp(CurrentPage, 1, TotalPages);

            StartPage = Math.Max(1, CurrentPage - 3);
            EndPage = Math.Min(TotalPages, CurrentPage + 3);

            if (EndPage - StartPage < 6)
            {
                if (CurrentPage < TotalPages / 2)
                    EndPage = Math.Min(StartPage + 6, TotalPages);
                else
                    StartPage = Math.Max(EndPage - 6, 1);
            }

            RawNewsItems = await query
                .OrderByDescending(m => m.CreatedAt)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }


        public string GetPageLink(int page)
        {
            var query = HttpContext.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
            query["currentPage"] = page.ToString();
            return $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}"))}";
        }



    }
}