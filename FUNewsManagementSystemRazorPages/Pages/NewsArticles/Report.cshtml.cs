using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.NewsArticles
{
    public class ReportModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public ReportModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public List<NewsReportDTO> ReportData { get; set; } = new List<NewsReportDTO>();

        public void OnGet()
        {
            if (StartDate == null || EndDate == null)
            {
                // Default: Last 30 days report
                StartDate = DateTime.UtcNow.AddDays(-30);
                EndDate = DateTime.UtcNow;
            }

            // Fetch data and filter by date range
            var articles = _newsArticleService.GetAllNewsArticles()
                .Where(a => a.CreatedDate >= StartDate && a.CreatedDate <= EndDate)
                .GroupBy(a => a.CreatedDate?.Date)
                .Select(g => new NewsReportDTO
                {
                    Date = g.Key ?? DateTime.UtcNow,
                    Count = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToList();

            ReportData = articles;
        }
    }

    public class NewsReportDTO
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
