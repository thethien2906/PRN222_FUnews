using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FUNewsManagementSystemRazorPages.Pages.SystemAccounts
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ILogger<CreateModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public SystemAccountInputModel SystemAccount { get; set; } = new SystemAccountInputModel();

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // 🚀 TODO: Lưu thông tin tài khoản vào database (hiện tại chỉ log ra)
            _logger.LogInformation("New Account Created: {AccountName}, {AccountEmail}, Role: {AccountRole}",
                SystemAccount.AccountName, SystemAccount.AccountEmail, SystemAccount.AccountRole);

            // Chuyển hướng về trang danh sách sau khi tạo thành công
            return RedirectToPage("/SystemAccounts/Index");
        }

        public class SystemAccountInputModel
        {
            [Required(ErrorMessage = "Account Name is required")]
            [StringLength(100, ErrorMessage = "Account Name must be less than 100 characters")]
            public string AccountName { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string AccountEmail { get; set; }

            [Required(ErrorMessage = "Please select a role")]
            public string AccountRole { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
            public string AccountPassword { get; set; }
        }
    }
}
