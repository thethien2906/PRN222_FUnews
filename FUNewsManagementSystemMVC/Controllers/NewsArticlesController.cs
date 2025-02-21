using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using Services.IService;
    
namespace FUNewsManagementSystemMVC.Controllers
{
    public class NewsArticlesController : Controller
    {
        //private readonly FunewsManagementContext _context;
        private readonly INewsArticleService _contextNewsArticle;
        private readonly ICategoryService _contextCategory;

        public NewsArticlesController(INewsArticleService contextNewsArticle, ICategoryService contextCategory)
        {
            _contextNewsArticle = contextNewsArticle;
            _contextCategory = contextCategory;
        }
        

        // GET: NewsArticles
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // Redirect to the login page or display an error message
                return RedirectToAction("Login", "SystemAccounts");
            }
            var funewsManagementContext = _contextNewsArticle.GetAllNewsArticles();
            return View(funewsManagementContext.ToList());
        }

        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _contextNewsArticle.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // GET: NewsArticles/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: NewsArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsArticle newsArticle)
        {
            ModelState.Remove("NewsArticleId");
            if (ModelState.IsValid)
            {
                try
                {
                    var lastArticle = _contextNewsArticle.GetAllNewsArticles().OrderByDescending(n => n.NewsArticleId).FirstOrDefault();
                    int newId = lastArticle != null ? int.Parse(lastArticle.NewsArticleId) + 1 : 1;
                    newsArticle.NewsArticleId = newId.ToString();

                    newsArticle.CreatedDate = DateTime.Now;

                    newsArticle.ModifiedDate = DateTime.Now;

                    var userId = HttpContext.Session.GetString("UserId");

                    newsArticle.CreatedById = short.Parse(userId);
                    newsArticle.UpdatedById = short.Parse(userId);

                    
                    _contextNewsArticle.CreateNewsArticle(newsArticle);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) here
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            return View(newsArticle);
        }
        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _contextNewsArticle.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            return View(newsArticle);
        }

        // POST: NewsArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NewsArticleId,NewsTitle,Headline,CreatedDate,NewsContent,NewsSource,CategoryId,NewsStatus,CreatedById,UpdatedById,ModifiedDate")] NewsArticle newsArticle)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    newsArticle.ModifiedDate = DateTime.Now;
                    var userId = HttpContext.Session.GetString("UserId");
                    newsArticle.UpdatedById = short.Parse(userId);

                    _contextNewsArticle.UpdateNewsArticle(newsArticle);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsArticleExists(newsArticle.NewsArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            return View(newsArticle);
        }

        // GET: NewsArticles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _contextNewsArticle.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var newsArticle = _contextNewsArticle.GetNewsArticleById(id);
            if (newsArticle != null)
            {
                _contextNewsArticle.DeleteNewsArticle(newsArticle);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NewsArticleExists(string id)
        {
            var tmp = _contextNewsArticle.GetNewsArticleById(id);
            return (tmp != null) ? true : false;
        }

        // GET: NewsArticles/Report
        public IActionResult Report(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                ViewData["Message"] = "Please select a valid date range.";
                return View(new List<NewsArticle>());
            }

            if (endDate < startDate)
            {
                ViewData["Message"] = "End date must be greater than start date.";
                return View(new List<NewsArticle>());
            }

            var articles = _contextNewsArticle.GetNewsArticlesByPeriod(startDate.Value, endDate.Value);
            return View(articles);
        }
    }
}

