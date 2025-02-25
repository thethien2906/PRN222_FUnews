using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Services.IService;
using Services.DTOs;
using Services.Service;
using BusinessObjects.Entities;

namespace FUNewsManagementSystemMVC.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        public NewsArticlesController(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        // GET: NewsArticles
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "SystemAccounts");
            }
            var articles = _newsArticleService.GetAllNewsArticles();
            return View(articles.ToList());
        }

        // GET: NewsArticles/History
        public IActionResult History()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "SystemAccounts");
            }

            var userId = short.Parse(HttpContext.Session.GetString("UserId"));
            var myArticles = _newsArticleService.GetNewsArticleByCreator(userId);
            return View(myArticles.ToList());
        }

        // GET: NewsArticles/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _newsArticleService.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // GET: NewsArticles/Create
        public IActionResult Create()
        {

            ViewData["CategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
            ViewData["Tags"] = new MultiSelectList(_tagService.GetAllTags(), "TagId", "TagName"); // Pass tags

            return View();
        }

        // POST: NewsArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("NewsTitle,Headline,NewsContent,NewsSource,CategoryId,NewsStatus,TagIds")] NewsArticleDTO newsArticleDto)
        {
            ModelState.Remove("NewsArticleId");
            if (ModelState.IsValid)
            {
                try
                {
                    var lastArticle = _newsArticleService.GetAllNewsArticles().OrderByDescending(n => n.NewsArticleId).FirstOrDefault();
                    int newId = lastArticle != null ? int.Parse(lastArticle.NewsArticleId) + 1 : 1;
                    newsArticleDto.NewsArticleId = newId.ToString();
                    newsArticleDto.CreatedDate = DateTime.Now;
                    newsArticleDto.ModifiedDate = DateTime.Now;

                    var userId = HttpContext.Session.GetString("UserId");
                    newsArticleDto.CreatedById = short.Parse(userId);
                    newsArticleDto.UpdatedById = short.Parse(userId);

                    _newsArticleService.CreateNewsArticle(newsArticleDto);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact support.");
                }
            }

            ViewData["CategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName", newsArticleDto.CategoryId);
            ViewData["Tags"] = new MultiSelectList(_tagService.GetAllTags(), "TagId", "TagName", newsArticleDto.TagIds); // Preserve selected tags

            return View(newsArticleDto);
        }

        // GET: NewsArticles/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _newsArticleService.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            ViewData["Tags"] = new MultiSelectList(
        _tagService.GetAllTags(),
        "TagId",
        "TagName",
        newsArticle.Tags.Select(t => t.TagId) // Pre-select existing tags
    );

            return View(newsArticle);
        }

        // POST: NewsArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("NewsArticleId,NewsTitle,Headline,NewsContent,NewsSource,CategoryId,NewsStatus,TagIds")] NewsArticleDTO newsArticleDto)
        {
            if (id != newsArticleDto.NewsArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingArticle = _newsArticleService.GetNewsArticleById(id);
                    if (existingArticle == null)
                    {
                        return NotFound();
                    }

                    newsArticleDto.CreatedDate = existingArticle.CreatedDate;
                    newsArticleDto.CreatedById = existingArticle.CreatedById;
                    newsArticleDto.ModifiedDate = DateTime.Now;

                    var userId = HttpContext.Session.GetString("UserId");
                    newsArticleDto.UpdatedById = short.Parse(userId);

                    _newsArticleService.UpdateNewsArticle(newsArticleDto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    if (!NewsArticleExists(newsArticleDto.NewsArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["CategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName", newsArticleDto.CategoryId);
            ViewData["Tags"] = new MultiSelectList(_tagService.GetAllTags(), "TagId", "TagName"); // Pass tags

            return View(newsArticleDto);
        }

        // GET: NewsArticles/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _newsArticleService.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var newsArticle = _newsArticleService.GetNewsArticleById(id);
            if (newsArticle != null)
            {
                _newsArticleService.DeleteNewsArticle(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NewsArticleExists(string id)
        {
            return _newsArticleService.GetNewsArticleById(id) != null;
        }

        // GET: NewsArticles/Report
        public IActionResult Report(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                ViewData["Message"] = "Please select a valid date range.";
                return View(new List<NewsArticleDTO>());
            }

            if (endDate < startDate)
            {
                ViewData["Message"] = "End date must be greater than start date.";
                return View(new List<NewsArticleDTO>());
            }

            var articles = _newsArticleService.GetNewsArticlesByPeriod(startDate.Value, endDate.Value).ToList(); // Convert to List
            return View(articles);
        }


    }
}
