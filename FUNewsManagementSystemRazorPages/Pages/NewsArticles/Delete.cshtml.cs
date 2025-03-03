using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public DeleteModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public NewsArticleDTO NewsArticle { get; set; }

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            NewsArticle = _newsArticleService.GetNewsArticleById(id);
            if (NewsArticle == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(NewsArticle.NewsArticleId))
            {
                return BadRequest();
            }

            var existingArticle = _newsArticleService.GetNewsArticleById(NewsArticle.NewsArticleId);
            if (existingArticle == null)
            {
                return NotFound();
            }

            _newsArticleService.DeleteNewsArticle(NewsArticle.NewsArticleId);

            return RedirectToPage("Index");
        }
    }
}
