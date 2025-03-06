using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Services.DTOs;
using Services.IService;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.SignalR;
using FUNewsManagementSystemRazorPages.Hubs;

namespace FUNewsManagementSystemRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IAccountService _accountService;
        private readonly IHubContext<SignalrServer> _hubContext;
        public IndexModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IAccountService accountService, IHubContext<SignalrServer> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _accountService = accountService;
            _hubContext = hubContext;
        }

        public IEnumerable<NewsArticleDTO> NewsArticles { get; set; } = new List<NewsArticleDTO>();
        public SelectList Categories { get; set; }
        public MultiSelectList Tags { get; set; }

        [BindProperty]
        public NewsArticleDTO NewArticle { get; set; }

        [BindProperty]
        public NewsArticleDTO EditArticle { get; set; }

        // Pagination properties
        public int PageSize { get; set; } = 5; // Number of articles per page
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var allArticles = _newsArticleService.GetActiveNewsArticles().OrderByDescending(a => a.CreatedDate).ToList();

            // Calculate pagination
            int totalArticles = allArticles.Count;
            TotalPages = (int)Math.Ceiling(totalArticles / (double)PageSize);
            CurrentPage = pageNumber;

            // Get paged list
            NewsArticles = allArticles.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            // Populate categories and tags
            Categories = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
            Tags = new MultiSelectList(_tagService.GetAllTags(), "TagId", "TagName");

            //SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate");
        }
    }
}
