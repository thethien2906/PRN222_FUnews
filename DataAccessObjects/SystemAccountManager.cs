using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class SystemAccountManager
    {
        private static SystemAccountManager instance = null;
        private static readonly object instanceLock = new object();
        private SystemAccountManager() { }

        public static SystemAccountManager Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SystemAccountManager();
                    }
                    return instance;
                }
            }
        }
        // Login
        public SystemAccount Authenticate(string email, string password)
        {
            try
            {
                using var _context = new FunewsManagementContext();

                // Tìm tài khoản theo email
                var account = _context.SystemAccounts
                    .SingleOrDefault(a => a.AccountEmail == email);

                if (account != null && account.AccountPassword == password) // So sánh mật khẩu (nên mã hóa mật khẩu trong thực tế)
                {
                    return account; // Trả về thông tin tài khoản nếu đăng nhập thành công
                }

                return null; // Nếu tài khoản không tồn tại hoặc mật khẩu sai
            }
            catch (Exception ex)
            {
                throw new Exception("Error during authentication", ex);
            }
        }
        public IEnumerable<SystemAccount> GetSystemAccountList()
        {
            try
            {
                using var _context = new FunewsManagementContext();
                return _context.SystemAccounts.Where(x => x.AccountRole.HasValue).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
         public SystemAccount GetSystemAccountById(int id)
        {
            SystemAccount systemAccount = null;
            try
            {
                using var _context = new FunewsManagementContext();
                systemAccount = _context.SystemAccounts.SingleOrDefault(a => a.AccountId == id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return systemAccount;
        }
        public void AddNew(SystemAccount systemAccount)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                _context.SystemAccounts.Add(systemAccount);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(SystemAccount systemAccount)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                _context.Entry(systemAccount).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(SystemAccount systemAccount)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                var existingSystemAccount = _context.SystemAccounts.SingleOrDefault(a => a.AccountId == systemAccount.AccountId);

                if (existingSystemAccount != null)
                {
                    _context.SystemAccounts.Remove(existingSystemAccount);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("SystemAccount does not exist.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error while deleting system account. Check inner exception for details.", ex);
            }
        }

        public void ChangeStatus(SystemAccount systemAccount)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                var existingSystemAccount = _context.SystemAccounts.FirstOrDefault(a => a.AccountId == systemAccount.AccountId);

                if (existingSystemAccount == null)
                {
                    throw new Exception("SystemAccount does not exist.");
                }
                else
                {
                    existingSystemAccount.AccountRole = existingSystemAccount.AccountRole.HasValue ? null : 1;
                    _context.Entry(existingSystemAccount).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<SystemAccount> Search(string keyword)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                return _context.SystemAccounts
                    .Where(a => (a.AccountName.ToLower().Contains(keyword.ToLower()) ||
                                 a.AccountEmail.ToLower().Contains(keyword.ToLower())) && a.AccountRole.HasValue)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //get account name by accountId
        public string GetAccountNameById(int id)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                return _context.SystemAccounts
                    .Where(a => a.AccountId == id)
                    .Select(a => a.AccountName)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}