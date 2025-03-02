using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.Tags
{
    public class DeleteModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public TagDTO Tag { get; set; }

        public DeleteModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IActionResult OnGet(int id)
        {
            Tag = _tagService.GetTagById(id);
            if (Tag == null)
            {
                TempData["ErrorMessage"] = "Tag not found.";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                _tagService.DeleteTag(Tag);
                TempData["SuccessMessage"] = "Tag deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting tag: {ex.Message}";
            }
            return RedirectToPage("Index");
        }
    }
}