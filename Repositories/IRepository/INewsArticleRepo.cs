using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;

namespace Repositories.IRepository
{
    public interface INewsArticleRepo
    {
        IEnumerable<NewsArticle> GetAll();
        NewsArticle GetNewsArticle(string id);
        void InsertNewsArticle(NewsArticle obj);
        void UpdateNewsArticle(NewsArticle obj);
        void DeleteNewsArticle(NewsArticle id);
        IEnumerable<NewsArticle> SearchNewsArticle(string search);
    }
}
