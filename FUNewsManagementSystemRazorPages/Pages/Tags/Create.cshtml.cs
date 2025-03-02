using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public TagDTO Tag { get; set; }

        public CreateModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public void OnGet()
        {
            Tag = new TagDTO();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _tagService.AddTag(Tag);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message); // Hiển thị thông báo lỗi từ TagManager
                return Page();
            }
        }
    }
}