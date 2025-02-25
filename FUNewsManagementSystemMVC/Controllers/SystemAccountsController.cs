using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.IService;
using Services.DTOs;

namespace FUNewsManagementSystemMVC.Controllers
{
    public class SystemAccountsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public SystemAccountsController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "SystemAccounts");
            }
            var account = _accountService.GetAccountById(int.Parse(userId));
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "SystemAccounts");
            }
            var account = _accountService.GetAccountById(int.Parse(userId));
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(SystemAccountDTO model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("UserId");
                var account = _accountService.GetAccountById(int.Parse(userId));
                if (account == null)
                {
                    return NotFound();
                }
                account.AccountName = model.AccountName;
                account.AccountEmail = model.AccountEmail;
                account.AccountPassword = model.AccountPassword;
                _accountService.UpdateAccount(account);
                return RedirectToAction("EditProfile", "SystemAccounts");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View(new SystemAccountDTO());
        }

        [HttpPost]
        public IActionResult Login(SystemAccountDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var adminEmail = _configuration["Admin:Account"];
            var adminPassword = _configuration["Admin:Pass"];
            if (model.AccountEmail == adminEmail && model.AccountPassword == adminPassword)
            {
                HttpContext.Session.SetString("UserId", "admin");
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetString("Role", "admin");
                return RedirectToAction("Admin", "Home");
            }
            var account = _accountService.GetAccounts()
                .FirstOrDefault(a => a.AccountEmail == model.AccountEmail && a.AccountPassword == model.AccountPassword);
            if (account != null)
            {
                HttpContext.Session.SetString("UserId", account.AccountId.ToString());
                HttpContext.Session.SetString("UserName", account.AccountName);
                HttpContext.Session.SetString("Role", account.AccountRole.ToString());
                return account.AccountRole switch
                {
                    3 => RedirectToAction("Staff", "Home"),
                    2 => RedirectToAction("Lecturer", "Home"),
                    _ => RedirectToAction("Index", "Home"),
                };
            }
            ViewBag.Error = "Invalid email or password";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "SystemAccounts");
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "SystemAccounts");
            }
            var accounts = _accountService.GetAccounts().ToList();
            return View(accounts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return NotFound();
            }
            var account = _accountService.GetAccountById(id.Value);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccountDTO account)
        {
            ModelState.Remove("AccountId");
            if (ModelState.IsValid)
            {
                var lastAccount = _accountService.GetAccounts().OrderByDescending(n => n.AccountId).FirstOrDefault();
                int newId = lastAccount != null ? lastAccount.AccountId + 1 : 1;
                account.AccountId = (short)newId;
                _accountService.SaveAccount(account);
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var account = _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("AccountId,AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccountDTO account)
        {
            if (id != account.AccountId || id <= 0)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.UpdateAccount(account);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while updating the account.");
                }
            }
            return View(account);
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var account = _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var account = _accountService.GetAccountById(id);
            if (account != null)
            {
                _accountService.DeleteAccount(id); // Sửa lại để truyền id
            }
            return RedirectToAction(nameof(Index));
        }
    }
}