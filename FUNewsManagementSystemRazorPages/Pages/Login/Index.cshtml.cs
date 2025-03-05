using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;
namespace FUNewsManagementSystemRazorPages.Pages.Login
{
    public class IndexModel : PageModel

    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        [BindProperty]
        public SystemAccountDTO Input { get; set; }

        public string ErrorMessage { get; set; }

        public IndexModel(IConfiguration configuration, IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }

        public void OnGet()
        {
            Input = new SystemAccountDTO();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var adminEmail = _configuration["Admin:Account"];
            var adminPassword = _configuration["Admin:Pass"];

            if (Input.AccountEmail == adminEmail && Input.AccountPassword == adminPassword)
            {
                HttpContext.Session.SetString("UserId", "admin");
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetString("Role", "admin");
                return RedirectToPage("/SystemAccounts/Index");
            }

            var account = _accountService.GetAccounts()
                .FirstOrDefault(a => a.AccountEmail == Input.AccountEmail && a.AccountPassword == Input.AccountPassword);

            if (account != null)
            {
                HttpContext.Session.SetString("UserId", account.AccountId.ToString());
                HttpContext.Session.SetString("Username", account.AccountName.ToString());
                HttpContext.Session.SetString("Role", account.AccountRole.ToString());

                return account.AccountRole switch
                {
                    1 => RedirectToPage("/Staff"),
                    2 => RedirectToPage("/Lecturer/Index"),
                    _ => RedirectToPage("/Index"),
                };
            }

            ErrorMessage = "Invalid email or password";
            return Page();
        }
    }
}
