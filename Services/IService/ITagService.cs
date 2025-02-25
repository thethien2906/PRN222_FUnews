using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using DataAccessObjects.Helper;
using Services.DTOs;


namespace Services.IService
{
    public interface ITagService
    {
        IEnumerable<TagDTO> GetAllTags();
        TagDTO GetTagById(int id);
        void AddTag(TagDTO tag);
        void UpdateTag(TagDTO tag);
        void DeleteTag(TagDTO tag);
        IEnumerable<TagDTO> GetTagsByArticleId(string articleId);
    }

}
