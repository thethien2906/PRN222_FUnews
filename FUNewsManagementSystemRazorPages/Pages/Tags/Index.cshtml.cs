using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;

        public IEnumerable<TagDTO> Tags { get; set; }

        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public void OnGet()
        {
            Tags = _tagService.GetAllTags();
        }
    }
}