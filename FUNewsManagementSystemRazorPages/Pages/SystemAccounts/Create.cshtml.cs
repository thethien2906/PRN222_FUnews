using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;
using System.Linq;
using System.Threading.Tasks;

namespace FUNewsManagementSystemRazorPages.Pages.SystemAccounts;

public class CreateModel : PageModel
{
    private readonly IAccountService _accountService;

    public CreateModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public SystemAccountDTO SystemAccount { get; set; } = new SystemAccountDTO();

    public void OnGet()
    {
        // Display the empty form
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Account.AccountId"); // Remove if AccountId is generated

        if (!ModelState.IsValid)
        {
            return Page(); // Return to the form if validation fails
        }

        var lastAccount = _accountService.GetAccounts().OrderByDescending(n => n.AccountId).FirstOrDefault();
        int newId = lastAccount != null ? lastAccount.AccountId + 1 : 1;
        SystemAccount.AccountId = (short)newId;

        _accountService.SaveAccount(SystemAccount);

        return RedirectToPage("Index"); // Redirect to the index page
    }
}
