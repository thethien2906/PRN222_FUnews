using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.SystemAccounts
{
    public class EditProfileModel : PageModel
    {
        private readonly IAccountService _accountService;

        public EditProfileModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public SystemAccountDTO SystemAccount { get; set; }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login/Index");
            }

            var account = _accountService.GetAccountById(int.Parse(userId));
            if (account == null)
            {
                return NotFound();
            }

            SystemAccount = account;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetString("UserId");
            var account = _accountService.GetAccountById(int.Parse(userId));

            if (account == null)
            {
                return NotFound();
            }

            account.AccountName = SystemAccount.AccountName;
            account.AccountEmail = SystemAccount.AccountEmail;
            account.AccountPassword = SystemAccount.AccountPassword;

            _accountService.UpdateAccount(account);
            HttpContext.Session.SetString("UserName", account.AccountName.ToString());


            return RedirectToPage();
        }
    }
}
