using BusinessObjects.Entities;
using Repositories.IRepository;
using Services.IService;
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
                // Không gán TagId vì nó thường do DB tự sinh
                TagName = tagDTO.TagName,
                Note = tagDTO.Note
            };
            _tagRepo.Add(tag);
        }

        public void UpdateTag(TagDTO tagDTO)
        {
            var tag = _tagRepo.Get(tagDTO.TagId); // Lấy đối tượng hiện có
            if (tag != null)
            {
                tag.TagName = tagDTO.TagName;
                tag.Note = tagDTO.Note;
                _tagRepo.Update(tag);
            }
        }

        public void DeleteTag(TagDTO tagDTO)
        {
            var tag = _tagRepo.Get(tagDTO.TagId); // Lấy đối tượng hiện có
            if (tag != null)
            {
                _tagRepo.Delete(tag);
            }
        }

        public IEnumerable<TagDTO> GetTagsByArticleId(string articleId) =>
            _tagRepo.GetTagsByArticleId(articleId).Select(tag => new TagDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName,
                Note = tag.Note // Sửa lỗi từ tagDTO.Note thành tag.Note
            });
    }
}