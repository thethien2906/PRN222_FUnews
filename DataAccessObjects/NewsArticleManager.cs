using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class NewsArticleManager
    {
        private static NewsArticleManager instance = null;
        private static readonly object instanceLock = new object();
        private NewsArticleManager() { }
        public static NewsArticleManager Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new NewsArticleManager();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<NewsArticle> GetNewsArticles()
        {
            List<NewsArticle> articles = new List<NewsArticle>();
            try
            {
                using var _context = new FunewsManagementContext();
                articles = _context.NewsArticles.Include(a => a.Category).Include(a => a.Tags).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return articles;
        }
        public NewsArticle GetNewsArticleById(string id)
        {
            NewsArticle article = null;
            try
            {
                using var _context = new FunewsManagementContext();
                article = _context.NewsArticles.Include(a => a.Category).Include(a => a.Tags).SingleOrDefault(a => a.NewsArticleId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return article;
        }
        public IEnumerable<NewsArticle> GetNewsArticleByCreator(short creatorId)
        {
            using var _context = new FunewsManagementContext();

            return _context.NewsArticles
            .Where(article => article.CreatedById == creatorId)
            .Include(article => article.Category)  
            .OrderByDescending(article => article.CreatedDate)
            .ToList();

        }
        public void AddNew(NewsArticle article)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                _context.NewsArticles.Add(article);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(NewsArticle article)
        {
            NewsArticle existingArticle = GetNewsArticleById(article.NewsArticleId);
            try
            {
                if (existingArticle != null)
                {
                    using var _context = new FunewsManagementContext();
                    _context.Entry(article).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The news article does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Remove(NewsArticle article)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                var existingArticle = _context.NewsArticles.SingleOrDefault(a => a.NewsArticleId == article.NewsArticleId);
                if (existingArticle != null)
                {
                    _context.NewsArticles.Remove(existingArticle);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("News article does not exist.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error while deleting news article. Check inner exception for details.", ex);
            }
        }
        public IEnumerable<NewsArticle> Search(string search)
        {
            List<NewsArticle> articles = new List<NewsArticle>();
            try
            {
                using var _context = new FunewsManagementContext();
                articles = _context.NewsArticles
                    .Where(a => a.NewsTitle.ToLower().Contains(search.ToLower()) || a.Headline.ToLower().Contains(search.ToLower()))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return articles;
        }

        public IEnumerable<NewsArticle> GetNewsArticlesByPeriod(DateTime startDate, DateTime endDate)
        {
            List<NewsArticle> articles = new List<NewsArticle>();
            try
            {
                using var _context = new FunewsManagementContext();
                articles = _context.NewsArticles
                    .Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate)
                    .OrderByDescending(a => a.CreatedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return articles;
        }

        public IEnumerable<NewsArticle> GetActiveNewsArticles()
        {
            List<NewsArticle> articles = new List<NewsArticle>();
            try
            {
                using var _context = new FunewsManagementContext();
                articles = _context.NewsArticles
                    .Where(a => a.NewsStatus == true)
                    .Include(a => a.Category)
                    .OrderByDescending(a => a.CreatedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return articles;
        }
    }
}