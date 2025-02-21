using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;


namespace Services.IService
{
    public interface INewsArticleService
    {
        IEnumerable<NewsArticle> GetAllNewsArticles();
        NewsArticle GetNewsArticleById(string id);
        void CreateNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(NewsArticle newsArticle);
        IEnumerable <NewsArticle> SearchNewsArticle(string search);
        IEnumerable<NewsArticle> GetNewsArticlesByPeriod(DateTime startDate, DateTime endDate);
        IEnumerable<NewsArticle> GetNewsArticleByCreator(short creatorId);
        IEnumerable<NewsArticle> GetActiveNewsArticles();
    }
}
