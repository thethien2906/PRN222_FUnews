using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;



namespace Services.Service
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepo _newsArticleRepo;
        public NewsArticleService(INewsArticleRepo newsArticleService)
        {
            _newsArticleRepo = newsArticleService;
        }

        public IEnumerable<NewsArticle> GetAllNewsArticles() => _newsArticleRepo.GetAll();
        public NewsArticle GetNewsArticleById(string id) => _newsArticleRepo.GetNewsArticle(id);

        public void CreateNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepo.InsertNewsArticle(newsArticle);
        }
        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepo.UpdateNewsArticle(newsArticle);
        }
        public void DeleteNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepo.DeleteNewsArticle(newsArticle);
        }
        public IEnumerable<NewsArticle> SearchNewsArticle(string search) => _newsArticleRepo.SearchNewsArticle(search);



    }
}
