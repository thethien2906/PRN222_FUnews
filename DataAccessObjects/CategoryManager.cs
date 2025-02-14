﻿using BusinessObjects.Entities;
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
                var _context = new FunewsManagementContext();
                categorys = _context.Categories.Where(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categorys;
        }
        public Category GetCategoryById(int id)
        {
            Category category = null;
            try
            {
                var _context = new FunewsManagementContext();
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
                var _context = new FunewsManagementContext();
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
                    var _context = new FunewsManagementContext();
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
                Category _category = GetCategoryById(category.CategoryId);
                if (_category != null)
                {
                    var _context = new FunewsManagementContext();
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Category does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ChangeStatus(Category category)
        {
            try
            {
                var _context = new FunewsManagementContext();
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
                var _context = new FunewsManagementContext();
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