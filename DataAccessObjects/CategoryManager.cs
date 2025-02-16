using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CategoryManager
    {
        private static CategoryManager instance = null;
        private static readonly object instanceLock = new object();
        private CategoryManager() { }
        public static CategoryManager Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryManager();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<Category> GetCategoryList()
        {
            List<Category> categorys = new List<Category>();
            try
            {
                using var _context = new FunewsManagementContext();
                categorys = _context.Categories.Where(x => x.IsActive == true).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return categorys;
        }
        public static void SaveCategory(Category category)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.Categories.Add(category);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Category GetCategoryById(int id)
        {
            Category category = null;
            try
            {
                using var _context = new FunewsManagementContext();
                category = _context.Categories.SingleOrDefault(category => category.CategoryId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;

        }
        public void AddNew(Category category)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(Category category)
        {
            Category _category = GetCategoryById(category.CategoryId);
            try
            {
                if (_category != null)
                {
                    using var _context = new FunewsManagementContext();
                    _context.Entry<Category>(category).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The category does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public void Remove(Category category)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                var existingCategory = _context.Categories
                    .Include(c => c.NewsArticles) // Include all related entities
                    .SingleOrDefault(c => c.CategoryId == category.CategoryId);

                if (existingCategory != null)
                {
                    _context.Categories.Remove(existingCategory);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Category does not exist.");
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception for more details
                throw new Exception("Error while deleting category. Check inner exception for details.", ex);
            }
        }

        public void ChangeStatus(Category category)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                var a = _context.Categories!.FirstOrDefault(c => c.CategoryId.Equals(category.CategoryId));


                if (a == null)
                {
                    throw new Exception("Category does not exist.");
                }
                else
                {
                    a.IsActive = false;
                    _context.Entry(a).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<Category> Search(string search)
        {
            List<Category> categorys = new List<Category>();
            try
            {
                using var _context = new FunewsManagementContext();
                categorys = _context.Categories
                    .Where(x => x.CategoryName.ToLower().Contains(search.ToLower()) && x.IsActive == true)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categorys;
        }
    }
}