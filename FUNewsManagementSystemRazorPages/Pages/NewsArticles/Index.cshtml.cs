using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Services.DTOs;
using Services.IService;
using Microsoft.AspNetCore.SignalR;
using FUNewsManagementSystemRazorPages.Hubs;

namespace FUNewsManagementSystemRazorPages.Pages.NewsArticles;

public class IndexModel : PageModel
{
    private readonly INewsArticleService _newsArticleService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;
    private readonly IHubContext<SignalrServer> _hubContext;

    public IndexModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IHubContext<SignalrServer> hubContext)
    {
        _newsArticleService = newsArticleService;
        _categoryService = categoryService;
        _tagService = tagService;
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
    public int PageSize { get; set; } = 5; // Number of items per page
    public int CurrentPage { get; set; } = 1;
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync(int pageNumber = 1)
    {
        // Ensure page number is valid
        CurrentPage = pageNumber > 0 ? pageNumber : 1;

        // Get all articles for counting
        var allArticles = _newsArticleService.GetAllNewsArticles().ToList();
        TotalItems = allArticles.Count;
        TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);

        // Sort by newest date first
        allArticles = allArticles
            .OrderByDescending(a => a.CreatedDate)
            .ToList();

        // Apply pagination
        NewsArticles = allArticles
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        Categories = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
        Tags = new MultiSelectList(_tagService.GetAllTags(), "TagId", "TagName");



        //SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("NewsArticleId");
        ModelState.Remove("Headline");
        ModelState.Remove("NewArticle.NewsArticleId");
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return Page();
        }
        if (!ModelState.IsValid)
        {
            Categories = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName", NewArticle.CategoryId);
            Tags = new MultiSelectList(_tagService.GetAllTags(), "TagId", "TagName", NewArticle.TagIds);
            return Page();
        }

        var lastArticle = _newsArticleService.GetAllNewsArticles()
            .AsEnumerable() // Switch to client-side evaluation
            .OrderByDescending(n => int.Parse(n.NewsArticleId))
            .FirstOrDefault();

        int newId = lastArticle != null ? int.Parse(lastArticle.NewsArticleId) + 1 : 1;
        NewArticle.NewsArticleId = newId.ToString();
        NewArticle.CreatedDate = DateTime.Now;
        NewArticle.ModifiedDate = DateTime.Now;

        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId) || !short.TryParse(userId, out short userIdShort))
        {
            userId = "1";
        }
        NewArticle.CreatedById = short.Parse(userId);
        NewArticle.UpdatedById = short.Parse(userId);

        _newsArticleService.CreateNewsArticle(NewArticle);
        // Trigger SignalR notification
        await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate");
        return RedirectToPage();
    }

    public IActionResult OnGetArticleData(string id)
    {
        var article = _newsArticleService.GetNewsArticleById(id);
        if (article == null)
        {
            return NotFound();
        }

        return new JsonResult(article);
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        ModelState.Remove("Headline");
        ModelState.Remove("NewsArticleId");
        // Explicitly check if required fields are present and valid
        if (string.IsNullOrEmpty(EditArticle.NewsArticleId))
        {
            ModelState.AddModelError("EditArticle.NewsArticleId", "Article ID is required.");
        }

        if (string.IsNullOrEmpty(EditArticle.Headline))
        {
            ModelState.AddModelError("EditArticle.Headline", "Headline is required.");
        }
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return Page();
        }
        if (!ModelState.IsValid)
        {
            Categories = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName", NewArticle.CategoryId);
            Tags = new MultiSelectList(_tagService.GetAllTags(), "TagId", "TagName", NewArticle.TagIds);
            return Page();
        }

        var existingArticle = _newsArticleService.GetNewsArticleById(EditArticle.NewsArticleId);
        if (existingArticle == null)
        {
            return NotFound();
        }

        // Preserve creation info
        EditArticle.CreatedDate = existingArticle.CreatedDate;
        EditArticle.CreatedById = existingArticle.CreatedById;

        // Update modification info
        EditArticle.ModifiedDate = DateTime.Now;

        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId) || !short.TryParse(userId, out short userIdShort))
        {
            userId = "1";
        }
        EditArticle.UpdatedById = short.Parse(userId);

        _newsArticleService.UpdateNewsArticle(EditArticle);

        await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate");

        return RedirectToPage();
    }
}