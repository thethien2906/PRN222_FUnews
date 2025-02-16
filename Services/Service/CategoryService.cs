using BusinessObjects.Entities;
using Repositories.IRepository;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        public CategoryService(ICategoryRepo categoryService)
        {
            _categoryRepo = categoryService;

        }

        public void ChangeStatus(Category category) => _categoryRepo.ChangeStatus(category);

        public void DeleteCategory(Category category)
        {
            _categoryRepo.DeleteCategory(category);
        }

        public Category GetCategoryByID(int id)
        {
            return _categoryRepo.GetCategoryByID(id);
        }

        public IEnumerable<Category> GetCategorys() => _categoryRepo.GetCategorys();

        public void InsertCategory(Category category)
        {
            _categoryRepo.InsertCategory(category);
        }

        public void SaveCategory(Category category)
        {
            if (category.CategoryId == 0)
            {
                _categoryRepo.InsertCategory(category);
            }
            else
            {
                _categoryRepo.UpdateCategory(category);
            }
        }


        public IEnumerable<Category> Search(string search) => _categoryRepo.Search(search);

        public void UpdateCategory(Category category)
        {
            _categoryRepo.UpdateCategory(category);
        }
    }
}