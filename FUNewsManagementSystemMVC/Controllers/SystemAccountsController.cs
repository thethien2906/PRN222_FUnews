using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using DataAccessObjects.Helpers;
using static BusinessObjects.Entities.SystemAccount;
using Services.IService;
using Microsoft.Extensions.Configuration;
using System.Configuration;

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
        // LOGIN
        public IActionResult Login()
        {
            return View(new SystemAccount());
        }

        [HttpPost]
        public IActionResult Login(SystemAccount model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra tài khoản admin từ appsettings.json
            var adminEmail = _configuration["Admin:Account"];
            var adminPassword = _configuration["Admin:Pass"];

            if (model.AccountEmail == adminEmail && model.AccountPassword == adminPassword)
            {
                // Lưu thông tin tài khoản admin vào session và điều hướng đến trang quản trị
                HttpContext.Session.SetString("UserId", "admin");
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetString("Role", "admin");

                return RedirectToAction("Admin", "Home");  // Định tuyến đến trang Admin
            }

            var account = _accountService.GetAccounts()
                .FirstOrDefault(a => a.AccountEmail == model.AccountEmail && a.AccountPassword == model.AccountPassword);

            if (account != null)
            {
                // Lưu thông tin tài khoản vào session
                HttpContext.Session.SetString("UserId", account.AccountId.ToString());
                HttpContext.Session.SetString("UserName", account.AccountName);
                HttpContext.Session.SetString("Role", account.AccountRole.ToString());

                // Điều hướng theo vai trò người dùng
                if (account.AccountRole == 1)
                {
                    return RedirectToAction("Staff", "Home");
                }
                else if (account.AccountRole == 2)
                {
                    return RedirectToAction("Lecturer", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = "Invalid email or password";
                return View(model);
            }
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "SystemAccounts");
        }
        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // Redirect to the login page or display an error message
                return RedirectToAction("Login", "SystemAccounts");
            }
            var accounts = _accountService.GetAccounts().ToList(); // Lấy tất cả tài khoản từ service
            return View(accounts);
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccount account)
        {
            ModelState.Remove("AccountId");
            if (ModelState.IsValid)
            {
                var lastAccount = _accountService.GetAccounts().OrderByDescending(n => n.AccountId).FirstOrDefault();
                int newId = lastAccount != null ? lastAccount.AccountId + 1 : 1;
                account.AccountId = (short)newId; _accountService.SaveAccount(account); // Lưu tài khoản mới
                return RedirectToAction(nameof(Index)); // Chuyển đến danh sách tài khoản
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccount account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.UpdateAccount(account); // Cập nhật tài khoản
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


            }
            return View(account);
        }
        private bool AccountExists(int id)
        {
            var tmp = _accountService.GetAccountById(id);
            return (tmp != null) ? true : false;
        }
        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
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

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = _accountService.GetAccountById(id);
            if (account != null)
            {
                _accountService.DeleteAccount(account); // Xóa tài khoản
            }
            return RedirectToAction(nameof(Index)); // Quay lại danh sách
        }
    }
}
