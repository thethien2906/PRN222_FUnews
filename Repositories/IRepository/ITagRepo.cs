using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using DataAccessObjects.Helper;
namespace Repositories.IRepository
{
    public interface ITagRepo
    {
        IEnumerable<Tag> GetAll();
        Tag Get(int id);
        void Add(Tag tag);
        void Update(Tag tag);
        void Delete(Tag tag);
        IEnumerable<Tag> GetTagsByArticleId(string articleId);
    }
}
