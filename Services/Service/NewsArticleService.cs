using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects.Entities;
using Repositories.IRepository;
using Services.DTOs;
using Services.IService;

namespace Services.Service
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepo _newsArticleRepo;
        private readonly ITagRepo _tagRepo;
        public NewsArticleService(INewsArticleRepo newsArticleRepo, ITagRepo tagRepo)
        {
            _newsArticleRepo = newsArticleRepo;
            _tagRepo = tagRepo;
        }

        public IEnumerable<NewsArticleDTO> GetAllNewsArticles() =>
            _newsArticleRepo.GetAll().Select(ConvertToDTO);

        public NewsArticleDTO GetNewsArticleById(string id) =>
            ConvertToDTO(_newsArticleRepo.GetNewsArticle(id));

        public void CreateNewsArticle(NewsArticleDTO newsArticleDto)
        {
            var selectedTags = _tagRepo.GetTagsByIds(newsArticleDto.TagIds ?? new List<int>()).ToList();

            var newsArticle = ConvertToEntity(newsArticleDto, selectedTags);
            _newsArticleRepo.InsertNewsArticle(newsArticle); // Save to DB
        }

        public void UpdateNewsArticle(NewsArticleDTO newsArticleDto)
        {
            var selectedTags = _tagRepo.GetTagsByIds(newsArticleDto.TagIds ?? new List<int>()).ToList();
            newsArticleDto.Tags.Clear();
            var newsArticle = ConvertToEntity(newsArticleDto, selectedTags);
            _newsArticleRepo.UpdateNewsArticle(newsArticle);
        }

        public void DeleteNewsArticle(string newsArticleId)
        {
            var article = _newsArticleRepo.GetNewsArticle(newsArticleId);
            if (article != null)
            {
                _newsArticleRepo.DeleteNewsArticle(article);
            }
        }

        public IEnumerable<NewsArticleDTO> SearchNewsArticle(string search) =>
            _newsArticleRepo.SearchNewsArticle(search).Select(ConvertToDTO);

        public IEnumerable<NewsArticleDTO> GetNewsArticlesByPeriod(DateTime startDate, DateTime endDate) =>
            _newsArticleRepo.GetNewsArticlesByPeriod(startDate, endDate).Select(ConvertToDTO);

        public IEnumerable<NewsArticleDTO> GetNewsArticleByCreator(short creatorId) =>
            _newsArticleRepo.GetNewsArticlesByCreatorId(creatorId).Select(ConvertToDTO);

        public IEnumerable<NewsArticleDTO> GetActiveNewsArticles() =>
            _newsArticleRepo.GetActiveNewsArticles().Select(ConvertToDTO);

        private NewsArticleDTO ConvertToDTO(NewsArticle article) => new NewsArticleDTO
        {
            NewsArticleId = article.NewsArticleId,
            NewsTitle = article.NewsTitle,
            Headline = article.Headline,
            CreatedDate = article.CreatedDate,
            NewsContent = article.NewsContent,
            NewsSource = article.NewsSource,
            CategoryId = article.CategoryId,
            NewsStatus = article.NewsStatus,
            CreatedById = article.CreatedById,
            UpdatedById = article.UpdatedById,
            ModifiedDate = article.ModifiedDate,
            CategoryName = article.Category?.CategoryName,
            CreatedByName = article.CreatedBy?.AccountName,
            Tags = article.Tags?.Select(tag => new TagDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName
            }).ToList() ?? new List<TagDTO>()
        };

        private NewsArticle ConvertToEntity(NewsArticleDTO dto, List<Tag> existingTags) => new NewsArticle
        {
            NewsArticleId = dto.NewsArticleId,
            NewsTitle = dto.NewsTitle,
            Headline = dto.Headline,
            CreatedDate = dto.CreatedDate,
            NewsContent = dto.NewsContent,
            NewsSource = dto.NewsSource,
            CategoryId = dto.CategoryId,
            NewsStatus = dto.NewsStatus,
            CreatedById = dto.CreatedById,
            UpdatedById = dto.UpdatedById,
            ModifiedDate = dto.ModifiedDate,
            Tags = existingTags
        };
    }
}
