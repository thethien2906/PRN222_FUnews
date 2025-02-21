using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;

namespace FUNewsManagementSystemMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _contextCategory;

        public CategoriesController(ICategoryService contextCategory)
        {
            _contextCategory = contextCategory;
        }

        // GET: Categories
        public IActionResult Index()
        {
            var categories = _contextCategory.GetCategorys();
            return View(categories.ToList());
        }

        // CREATE CATEGORY - SHOW POPUP FORM
        public IActionResult Create()
        {
            ViewData["ParentCategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryName");
            return PartialView("_CreateEdit", new Category()); // Load partial view in popup
        }

        // CREATE CATEGORY - SAVE DATA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CategoryId,CategoryName,CategoryDesciption,ParentCategoryId,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                _contextCategory.SaveCategory(category);
                TempData["SuccessMessage"] = "Category created successfully.";
                return Json(new { success = true }); // Return success response
            }

            ViewData["ParentCategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return PartialView("_CreateEdit", category);
        }

        // EDIT CATEGORY - SHOW POPUP FORM
        public IActionResult Edit(int id)
        {
            var category = _contextCategory.GetCategoryByID(id);
            if (category == null) return NotFound();

            ViewData["ParentCategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return PartialView("_CreateEdit", category);
        }

        // EDIT CATEGORY - SAVE DATA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CategoryId,CategoryName,CategoryDesciption,ParentCategoryId,IsActive")] Category category)
        {
            if (id != category.CategoryId) return NotFound();

            if (ModelState.IsValid)
            {
                _contextCategory.UpdateCategory(category);
                TempData["SuccessMessage"] = "Category updated successfully.";
                return Json(new { success = true });
            }

            ViewData["ParentCategoryId"] = new SelectList(_contextCategory.GetCategorys(), "CategoryId", "CategoryName", category.ParentCategoryId);
            return PartialView("_CreateEdit", category);
        }

        // DELETE CATEGORY - SHOW CONFIRMATION PAGE
        public IActionResult Delete(int id)
        {
            var category = _contextCategory.GetCategoryByID(id);
            if (category == null) return NotFound();

            return PartialView("_Delete", category);
        }

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