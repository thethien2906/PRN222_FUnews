using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.NewsArticles
{
    public class ReportModel : PageModel // Đổi tên class
    {
        private readonly INewsArticleService _newsArticleService;

        public ReportModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public List<NewsArticleDTO> NewsArticles { get; set; } = new List<NewsArticleDTO>();

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; } = DateTime.Now;

        public void OnGet()
        {
            NewsArticles = _newsArticleService.GetNewsArticlesByPeriod(StartDate, EndDate)
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
        }
    }
}
