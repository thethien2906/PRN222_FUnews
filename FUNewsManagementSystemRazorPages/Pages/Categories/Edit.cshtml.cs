using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using Services.DTOs;

namespace FUNewsManagementSystemRazorPages.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public CategoryDTO Category { get; set; }

        public IActionResult OnGet(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = _categoryService.GetCategoryById(id.Value);
            if (Category == null)
            {
                return NotFound();
            }

            ViewData["ParentCategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ParentCategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
                return Page();
            }

            _categoryService.UpdateCategory(Category);
            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}