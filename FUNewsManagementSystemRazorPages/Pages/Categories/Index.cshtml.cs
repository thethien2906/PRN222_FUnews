using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using Services.DTOs;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FUNewsManagementSystemRazorPages.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IList<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public IActionResult OnGet()
        {
            //Comment dòng dưới để test mà không cần đăng nhập
            // Check if the session contains a valid account
/*            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Account")))
            {
                // If no session, redirect to the login page
                return RedirectToPage("/Login");
            }*/

            // If session is valid, load the categories
            Categories = string.IsNullOrEmpty(SearchTerm)
                ? _categoryService.GetCategories().ToList()
                : _categoryService.Search(SearchTerm).ToList();

            return Page();
        }

        public JsonResult OnGetEdit(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return new JsonResult(new { success = false, message = "Category not found" });
            }

            return new JsonResult(new
            {
                success = true,
                categoryId = category.CategoryId,
                categoryName = category.CategoryName,
                categoryDescription = category.CategoryDesciption,
                parentCategoryId = category.ParentCategoryId
            });
        }
    }
}