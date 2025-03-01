using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Services.IService;
using Services.DTOs;
using FUNewsManagementSystemRazorPages.Hubs;

namespace FUNewsManagementSystemRazorPages.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IHubContext<SignalrServer> _hubContext;
        public CreateModel(ICategoryService categoryService, IHubContext<SignalrServer> hubContext)
        {
            _categoryService = categoryService;
            _hubContext = hubContext; // Inject the SignalR Hub context
        }

        [BindProperty]
        public CategoryDTO Category { get; set; } = new();

        public IActionResult OnGet()
        {
            ViewData["ParentCategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ParentCategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
                return Page();
            }

            _categoryService.InsertCategory(Category);
            TempData["SuccessMessage"] = "Category created successfully!";

            // Notify all clients via SignalR
            await _hubContext.Clients.All.SendAsync("LoadAllItems");

            return RedirectToPage("./Index");
        }
    }
}