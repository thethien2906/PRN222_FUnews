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
using Services.DTOs;

namespace Services.Service
{
    public class TagService : ITagService
    {
        private readonly ITagRepo _tagRepo;
        public TagService(ITagRepo tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public IEnumerable<TagDTO> GetAllTags() =>
            _tagRepo.GetAll().Select(tag => new TagDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName,
                Note = tag.Note
            });

        public TagDTO GetTagById(int id)
        {
            var tag = _tagRepo.Get(id);
            return tag != null ? new TagDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName,
                Note = tag.Note
            } : null;
        }

        public void AddTag(TagDTO tagDTO)
        {
            var tag = new Tag
            {
                TagId = tagDTO.TagId,
                TagName = tagDTO.TagName,
                Note = tagDTO.Note
            };
            _tagRepo.Add(tag);
        }

        public void UpdateTag(TagDTO tagDTO)
        {
            var tag = new Tag
            {
                TagId = tagDTO.TagId,
                TagName = tagDTO.TagName,
                Note = tagDTO.Note
            };
            _tagRepo.Update(tag);
        }

        public void DeleteTag(TagDTO tagDTO)
        {
            var tag = new Tag
            {
                TagId = tagDTO.TagId,
                TagName = tagDTO.TagName,
                Note = tagDTO.Note
            };
            _tagRepo.Delete(tag);
        }

        public IEnumerable<TagDTO> GetTagsByArticleId(string articleId) =>
            _tagRepo.GetTagsByArticleId(articleId).Select(tag => new TagDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName,
                Note = tag.Note
            });
    }

}
