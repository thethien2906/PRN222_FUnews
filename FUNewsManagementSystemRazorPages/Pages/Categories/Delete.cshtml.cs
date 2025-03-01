using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using Services.DTOs;
using System;

namespace FUNewsManagementSystemRazorPages.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public DeleteModel(ICategoryService categoryService)
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

            return Page();
        }

        public IActionResult OnPost(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                _categoryService.DeleteCategory(id.Value);
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "This category is linked to news articles and cannot be deleted.";
            }

            return RedirectToPage("./Index");
        }
    }
}