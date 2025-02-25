using Repositories.IRepository;
using Services.IService;
using Services.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryService(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            return _categoryRepo.GetCategories().Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDesciption = c.CategoryDesciption,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive,
                ParentCategoryName = c.ParentCategory?.CategoryName
            }).ToList();
        }

        public CategoryDTO GetCategoryById(int id)
        {
            var category = _categoryRepo.GetCategoryByID(id);
            if (category == null) return null;

            return new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive,
                ParentCategoryName = category.ParentCategory?.CategoryName
            };
        }

        public void InsertCategory(CategoryDTO categoryDTO)
        {
            var category = new BusinessObjects.Entities.Category
            {
                CategoryName = categoryDTO.CategoryName,
                CategoryDesciption = categoryDTO.CategoryDesciption,
                ParentCategoryId = categoryDTO.ParentCategoryId,
                IsActive = categoryDTO.IsActive
            };

            _categoryRepo.InsertCategory(category);
        }

        public void UpdateCategory(CategoryDTO categoryDTO)
        {
            var category = _categoryRepo.GetCategoryByID(categoryDTO.CategoryId);
            if (category != null)
            {
                category.CategoryName = categoryDTO.CategoryName;
                category.CategoryDesciption = categoryDTO.CategoryDesciption;
                category.ParentCategoryId = categoryDTO.ParentCategoryId;
                category.IsActive = categoryDTO.IsActive;

                _categoryRepo.UpdateCategory(category);
            }
        }

        public void SaveCategory(CategoryDTO categoryDTO)
        {
            if (categoryDTO.CategoryId <= 0)
            {
                InsertCategory(categoryDTO);
            }
            else
            {
                UpdateCategory(categoryDTO);
            }
        }


        public void DeleteCategory(int id)
        {
            var category = _categoryRepo.GetCategoryByID(id);
            if (category != null)
            {
                _categoryRepo.DeleteCategory(category);
            }
        }

        public void ChangeStatus(int id)
        {
            var category = _categoryRepo.GetCategoryByID(id);
            if (category != null)
            {
                category.IsActive = !category.IsActive;
                _categoryRepo.UpdateCategory(category);
            }
        }

        public IEnumerable<CategoryDTO> Search(string search)
        {
            return _categoryRepo.Search(search).Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDesciption = c.CategoryDesciption,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive,
                ParentCategoryName = c.ParentCategory?.CategoryName
            }).ToList();
        }
    }
}
