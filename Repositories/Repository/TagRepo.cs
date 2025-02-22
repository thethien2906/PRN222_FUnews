using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepository;
using DataAccessObjects.Helper;

namespace Repositories.Repository
{
    public class TagRepo : ITagRepo
    {
        public IEnumerable<Tag> GetAll() => TagManager.Instance.GetTags();
        public Tag Get(int id) => TagManager.Instance.GetTagById(id);
        public void Add(Tag tag) => TagManager.Instance.AddNew(tag);
        public void Update(Tag tag) => TagManager.Instance.Update(tag);
        public void Delete(Tag tag) => TagManager.Instance.Delete(tag);
        public IEnumerable<Tag> GetTagsByArticleId(string id) => TagManager.Instance.GetTagsByNewsArticle(id);
    }
}
