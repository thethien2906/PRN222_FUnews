using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using Services.DTOs;

namespace Services.IService
{
    public interface INewsArticleService
    {
        IEnumerable<NewsArticleDTO> GetAllNewsArticles();
        NewsArticleDTO GetNewsArticleById(string id);
        void CreateNewsArticle(NewsArticleDTO newsArticle);
        void UpdateNewsArticle(NewsArticleDTO newsArticle);
        void DeleteNewsArticle(string newsArticleId);
        IEnumerable<NewsArticleDTO> SearchNewsArticle(string search);
        IEnumerable<NewsArticleDTO> GetNewsArticlesByPeriod(DateTime startDate, DateTime endDate);
        IEnumerable<NewsArticleDTO> GetNewsArticleByCreator(short creatorId);
        IEnumerable<NewsArticleDTO> GetActiveNewsArticles();
    }

}
