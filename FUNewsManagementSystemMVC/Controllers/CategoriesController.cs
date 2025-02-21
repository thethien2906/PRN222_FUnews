﻿using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace FUNewsManagementSystemMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly FunewsManagementContext _context;
        private readonly ICategoryService _contextCategory;
        public CategoriesController(ICategoryService contextCategory)
        {
            _contextCategory = contextCategory;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // Redirect to the login page or display an error message
                return RedirectToAction("Login", "SystemAccounts");
            }
            var funewsManagementContext = _contextCategory.GetCategorys();
            return View(funewsManagementContext.ToList());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _contextCategory.GetCategoryByID((int)id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["ParentCategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryDesciption");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryDesciption,ParentCategoryId,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                _contextCategory.SaveCategory(category);
            }
            ViewData["ParentCategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryDesciption", category.ParentCategoryId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _contextCategory.GetCategoryByID((int)id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryDesciption", category.ParentCategoryId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("CategoryId,CategoryName,CategoryDesciption,ParentCategoryId,IsActive")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contextCategory.UpdateCategory(category);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption", category.ParentCategoryId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _contextCategory.GetCategoryByID((int)id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var category = _contextCategory.GetCategoryByID((int)id);
            if (category != null)
            {
                _contextCategory.DeleteCategory(category);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(short id)
        {
            var tmp = _contextCategory.GetCategoryByID(id);
            return (tmp != null) ? true : false;
        }
    }
}
