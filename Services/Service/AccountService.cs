using BusinessObjects.Entities;
using Repositories.IRepository;
using Services.IService;
using System;
using System.Collections.Generic;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;

        public AccountService(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public void ChangeStatus(SystemAccount account) => _accountRepo.ChangeStatus(account);

        public void DeleteAccount(SystemAccount account)
        {
            _accountRepo.DeleteAccount(account);
        }

        public SystemAccount GetAccountByID(int id)
        {
            return _accountRepo.GetAccountByID(id);
        }

        public IEnumerable<SystemAccount> GetAccounts() => _accountRepo.GetAccounts();

        public void InsertAccount(SystemAccount account)
        {
            _accountRepo.InsertAccount(account);
        }

        public void SaveAccount(SystemAccount account)
        {
            if (account.AccountId == 0)
            {
                _accountRepo.InsertAccount(account);
            }
            else
            {
                _accountRepo.UpdateAccount(account);
            }
        }

        public IEnumerable<SystemAccount> Search(string search) => _accountRepo.Search(search);

        public void UpdateAccount(SystemAccount account)
        {
            _accountRepo.UpdateAccount(account);
        }
    }
}
