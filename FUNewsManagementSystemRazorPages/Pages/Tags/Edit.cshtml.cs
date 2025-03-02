using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.Tags
{
    public class EditModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public TagDTO Tag { get; set; }

        public EditModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IActionResult OnGet(int id)
        {
            Tag = _tagService.GetTagById(id);
            if (Tag == null)
            {
                return NotFound(); // Tr? v? 404 n?u không tìm th?y Tag
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Tr? l?i trang n?u d? li?u không h?p l?
            }

            _tagService.UpdateTag(Tag); // C?p nh?t Tag
            return RedirectToPage("Index"); // Chuy?n h??ng v? danh sách sau khi l?u
        }
    }
}