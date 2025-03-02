using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.Tags
{
    public class DetailsModel : PageModel
    {
        private readonly ITagService _tagService;

        public TagDTO Tag { get; set; }

        public DetailsModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IActionResult OnGet(int id)
        {
            Tag = _tagService.GetTagById(id);
            if (Tag == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy Tag
            }
            return Page();
        }
    }
}