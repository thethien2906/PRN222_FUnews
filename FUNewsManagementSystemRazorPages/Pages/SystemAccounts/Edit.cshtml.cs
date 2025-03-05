using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using Services.DTOs;

namespace FUNewsManagementSystemRazorPages.Pages.SystemAccounts
{
    public class EditModel : PageModel
    {
        private readonly IAccountService _accountService;

        public EditModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Trả về trang hiện tại với lỗi validation
            }

            try
            {
                // Nếu mật khẩu để trống, giữ nguyên mật khẩu cũ
                if (string.IsNullOrWhiteSpace(Account.AccountPassword))
                {
                    var existingAccount = _accountService.GetAccountById(Account.AccountId);
                    if (existingAccount != null)
                    {
                        Account.AccountPassword = existingAccount.AccountPassword;
                    }
                }

                _accountService.UpdateAccount(Account);
                return RedirectToPage("/SystemAccounts/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}