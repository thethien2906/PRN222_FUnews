using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;
using DataAccessObjects.Helper;

namespace Services.Service
{
    public class TagService : ITagService
    {
        private readonly ITagRepo _tagRepo;
        public TagService(ITagRepo tagService)
        {
            _tagRepo = tagService;
        }
        public IEnumerable<Tag> GetAllTags() => _tagRepo.GetAll();
        public Tag GetTagById(int id) => _tagRepo.Get(id);
        public void AddTag(Tag tag) => _tagRepo.Add(tag);
        public void UpdateTag(Tag tag) => _tagRepo.Update(tag);
        public ServiceResponse DeleteTag(Tag tag) => _tagRepo.Delete(tag);
        public IEnumerable<Tag> GetTagsByArticleId(string id) => _tagRepo.GetTagsByArticleId(id);
    }
}
