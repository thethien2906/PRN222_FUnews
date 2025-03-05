using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;
using System.Threading.Tasks;
namespace FUNewsManagementSystemRazorPages.Pages.SystemAccounts;

public class DeleteModel : PageModel
{
    private readonly IAccountService _accountService;

    public DeleteModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public SystemAccountDTO SystemAccount { get; set; }

    public IActionResult OnGet(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        SystemAccount = _accountService.GetAccountById(id);

        if (SystemAccount == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPostAsync(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        var account = _accountService.GetAccountById(id);

        if (account != null)
        {
            _accountService.DeleteAccount(id);
        }

        return RedirectToPage("Index");
    }
}
