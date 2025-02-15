using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategorys();
        Category GetCategoryByID(int id);
        void InsertCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        void ChangeStatus(Category category);
        IEnumerable<Category> Search(string search);
    }
}