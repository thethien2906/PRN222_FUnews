using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace FUNewsManagementSystemRazorPages.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult OnPost()
        {
            var user = _accountService.Authenticate(Email, Password);

            if (user != null)
            {
                // Kiểm tra nếu AccountRole là null, gán giá trị mặc định là 0
                int userRole = user.AccountRole ?? 0;

                // Lưu thông tin user vào session
                HttpContext.Session.SetString("UserEmail", user.AccountEmail);
                HttpContext.Session.SetString("UserName", user.AccountName);

                // Nếu là Staff (1) hoặc Lecturer (2), lưu thêm UserRole
                if (userRole == 1 || userRole == 2)
                {
                    HttpContext.Session.SetInt32("UserRole", userRole);
                }
                //if (userRole == 1) // Là Staff
                //{
                //    return RedirectToPage("/Index"); 
                //}
                //if (userRole == 2) // Là Manager
                //{
                //    return RedirectToPage("/Index"); 
                //}
                return RedirectToPage("/SystemAccount/Index");
            }
            else
            {
                ErrorMessage = "Invalid email or password!";
                return Page();
            }
        }
    }
}
