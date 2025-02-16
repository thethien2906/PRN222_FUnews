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
        public NewsArticlesController(INewsArticleService contextNewsArticle)
        {
            _contextNewsArticle = contextNewsArticle;
        }

        // GET: NewsArticles
        public async Task<IActionResult> Index()
        {
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
            //ViewData["CategoryId"] = new SelectList(_contextNewsArticle.GetAllNewsArticles(), "CategoryId", "CategoryDesciption");
            //ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "AccountId", "AccountId");
            ViewData["NewsArticleId"] = new SelectList(_contextNewsArticle.GetAllNewsArticles(), "NewsArticleId", "NewsTitle");
            return View();
        }

        // POST: NewsArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsArticleId,NewsTitle,Headline,CreatedDate,NewsContent,NewsSource,CategoryId,NewsStatus,CreatedById,UpdatedById,ModifiedDate")] NewsArticle newsArticle)
        {
            if (ModelState.IsValid)
            {
                _contextNewsArticle.CreateNewsArticle(newsArticle);
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            //ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "AccountId", "AccountId", newsArticle.CreatedById);
            ViewData["NewsArticleId"] = new SelectList(_contextNewsArticle.GetAllNewsArticles(), "NewsArticleId", "NewsTitle", newsArticle.NewsArticleId);
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
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            //ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "AccountId", "AccountId", newsArticle.CreatedById);
            ViewData["NewsArticleId"] = new SelectList(_contextNewsArticle.GetAllNewsArticles(), "NewsArticleId", "NewsTitle", newsArticle.NewsArticleId);
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
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            //ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "AccountId", "AccountId", newsArticle.CreatedById);
            ViewData["NewsArticleId"] = new SelectList(_contextNewsArticle.GetAllNewsArticles(), "NewsArticleId", "NewsTitle", newsArticle.NewsArticleId);
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
    }
}
