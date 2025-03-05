using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using Services.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUNewsManagementSystemRazorPages.Pages.SystemAccounts
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _systemAccountService;

        public IEnumerable<SystemAccountDTO> Accounts { get; set; }

        public IndexModel(IAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public void OnGet()
        {
            Accounts = _systemAccountService.GetAccounts();
        }
    }
}