using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystemRazorPages.Pages.Staff
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToPage("/Login/Index");
            }
            // add a return statement if needed
            return Page();
        }
    }
}
