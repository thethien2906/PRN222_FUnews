using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.NewsArticles
{
    public class HistoryModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public HistoryModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public List<NewsArticleDTO> MyArticles { get; set; } = new List<NewsArticleDTO>();

        public IActionResult OnGet()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToPage("/Login/Index");
            }

            if (short.TryParse(userIdString, out short userId))
            {
                MyArticles = _newsArticleService.GetNewsArticleByCreator(userId).ToList();
            }

            return Page();
        }
    }
}
