using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.SystemAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly IAccountService _systemAccountService;

        [BindProperty]
        public SystemAccountDTO SystemAccount { get; set; }

        public DeleteModel(IAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public IActionResult OnGet(int id)
        {
            SystemAccount = _systemAccountService.GetAccountById(id);
            if (SystemAccount == null)
            {
                TempData["ErrorMessage"] = "Account not found.";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                _systemAccountService.DeleteAccount(SystemAccount.AccountId);
                TempData["SuccessMessage"] = "System account deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting account: {ex.Message}";
            }
            return RedirectToPage("Index");
        }
    }
}
