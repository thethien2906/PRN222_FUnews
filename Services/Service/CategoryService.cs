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
        private readonly ICategoryRepo _categoryService;
        public CategoryService(ICategoryRepo categoryService)
        {
            _categoryService = categoryService;

        }

        public void ChangeStatus(Category category) => _categoryService.ChangeStatus(category);

        public void DeleteCategory(Category category)
        {
            _categoryService.DeleteCategory(category);
        }

        public Category GetCategoryByID(int id)
        {
            return _categoryService.GetCategoryByID(id);
        }

        public IEnumerable<Category> GetCategorys() => _categoryService.GetCategorys();

        public void InsertCategory(Category category)
        {
            _categoryService.InsertCategory(category);
        }

        public IEnumerable<Category> Search(string search) => _categoryService.Search(search);

        public void UpdateCategory(Category category)
        {
            _categoryService.UpdateCategory(category);
        }
    }
}