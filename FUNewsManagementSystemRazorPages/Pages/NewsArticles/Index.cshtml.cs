using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.NewsArticles;

public class IndexModel : PageModel
{
    private readonly INewsArticleService _newsArticleService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;

    public IndexModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService)
    {
        _newsArticleService = newsArticleService;
        _categoryService = categoryService;
        _tagService = tagService;
    }

    public IEnumerable<NewsArticleDTO> NewsArticles { get; set; } = new List<NewsArticleDTO>();
    public SelectList Categories { get; set; }
    public MultiSelectList Tags { get; set; }

    [BindProperty]
    public NewsArticleDTO NewArticle { get; set; }
    [BindProperty]
    public NewsArticleDTO EditArticle { get; set; }
    public void OnGet()
    {
        NewsArticles = _newsArticleService.GetAllNewsArticles();
        Categories = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
        Tags = new MultiSelectList(_tagService.GetAllTags(), "TagId", "TagName");

    }

    public IActionResult OnPost()
    {
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
            .FirstOrDefault(); int newId = lastArticle != null ? int.Parse(lastArticle.NewsArticleId) + 1 : 1;
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

    public IActionResult OnPostEdit()
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

        return RedirectToPage();
    }
}