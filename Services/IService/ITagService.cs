using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using DataAccessObjects.Helper;
namespace Services.IService
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAllTags();
        Tag GetTagById(int id);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        ServiceResponse DeleteTag(Tag tag);
        IEnumerable<Tag> GetTagsByArticleId(string articleId);
    }
}
