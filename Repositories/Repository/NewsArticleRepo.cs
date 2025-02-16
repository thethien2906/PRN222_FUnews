using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepository;


namespace Repositories.Repository
{
    public class NewsArticleRepo : INewsArticleRepo
    {
        public IEnumerable<NewsArticle> GetAll() => NewsArticleManager.Instance.GetNewsArticles();
        public NewsArticle GetNewsArticle(string id) => NewsArticleManager.Instance.GetNewsArticleById(id);
        public void InsertNewsArticle(NewsArticle newsArticle) => NewsArticleManager.Instance.AddNew(newsArticle);
        public void UpdateNewsArticle(NewsArticle newsArticle) => NewsArticleManager.Instance.Update(newsArticle);
        public void DeleteNewsArticle(NewsArticle newsArticle) => NewsArticleManager.Instance.Remove(newsArticle);
        public IEnumerable<NewsArticle> SearchNewsArticle(string search) => NewsArticleManager.Instance.Search(search);
    }
}
