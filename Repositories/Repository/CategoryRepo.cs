using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        public void ChangeStatus(Category category) => CategoryManager.Instance.ChangeStatus(category);

        public void DeleteCategory(Category category) => CategoryManager.Instance.Remove(category);

        public Category GetCategoryByID(int id) => CategoryManager.Instance.GetCategoryById(id);

        public IEnumerable<Category> GetCategories() => CategoryManager.Instance.GetCategoryList();

        public void InsertCategory(Category category) => CategoryManager.Instance.AddNew(category);

        public IEnumerable<Category> Search(string search) => CategoryManager.Instance.Search(search);

        public void UpdateCategory(Category category) => CategoryManager.Instance.Update(category);
    }
}