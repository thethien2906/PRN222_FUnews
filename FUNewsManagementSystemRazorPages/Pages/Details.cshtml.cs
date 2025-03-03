using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;
using System;

namespace FUNewsManagementSystemRazorPages.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IAccountService _accountService;

        public DetailsModel(INewsArticleService newsArticleService, IAccountService accountService)
        {
            _newsArticleService = newsArticleService;
            _accountService = accountService;
        }

        public NewsArticleDTO NewsArticle { get; set; }

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            NewsArticle = _newsArticleService.GetNewsArticleById(id);
            NewsArticle.CreatedByName = _accountService.GetAccountNameById((int)NewsArticle.CreatedById);
            if (NewsArticle == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}