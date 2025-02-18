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
    }
}
