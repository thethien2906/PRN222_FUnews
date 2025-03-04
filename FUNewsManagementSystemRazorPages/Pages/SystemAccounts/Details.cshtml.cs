using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using Services.DTOs;

namespace FUNewsManagementSystemRazorPages.Pages.SystemAccounts
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;

        public DetailsModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public SystemAccountDTO Account { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                Account = _accountService.GetAccountById(id);
                if (Account == null)
                {
                    return NotFound();
                }
                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return RedirectToPage("/SystemAccounts/Index");
            }
        }
    }
}